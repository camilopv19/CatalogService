using BusinessLogicLayer.CoreLogic;
using BusinessLogicLayer.Identity;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

string MANAGER_ROLE = "Manager";
string BUYER_ROLE = "Buyer";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        options.Audience = "api1";
//        options.Authority = "https://localhost:7297";
//    })
//    .AddCookie("cookie");
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(options =>
     {
         options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
         options.DefaultChallengeScheme = "oidc";
     })
    .AddCookie()
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:7297";
        options.ClientId = "oauthClient";
        options.ClientSecret = "client_secret";
        options.ResponseType = "code";
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.SaveTokens = true;
    });
builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy("ManagerRole", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser()
        .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
        .RequireClaim("role", MANAGER_ROLE);
    });
    builder.AddPolicy("BuyerRole", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser()
        .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
        .RequireClaim("role", BUYER_ROLE);
    });
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog API", Version = "v1" });
    // Include XML comments
    options.IncludeXmlComments(".\\obj\\Debug\\net6.0\\CatalogService.xml");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
