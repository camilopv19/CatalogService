using Identity;
using Identity.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(config =>
{
    config.UseInMemoryDatabase("Memory");
});
builder.Services.AddControllers();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
//{
//    config.Password.RequiredLength = 4;
//    config.Password.RequireDigit = false;
//    config.Password.RequireNonAlphanumeric = false;
//    config.Password.RequireUppercase = false;
//    config.SignIn.RequireConfirmedEmail = true;
//})
//.AddEntityFrameworkStores<AppDbContext>()
//.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "IdentityServer.Cookie";
    config.LoginPath = "/Auth/Login";
});


// using local db (assumes Visual Studio has been installed)
const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;database=Test.IdentityServer.EntityFramework;trusted_connection=yes;";
var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
//builder.Services.AddDbContext<AppDbContext>(builder =>
//    builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders(); ;

//Identity server
builder.Services.AddIdentityServer()
    .AddInMemoryClients(IdentityServerConfig.GetClients())
    .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
    .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
    .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
    .AddDeveloperSigningCredential()
    .AddTestUsers(IdentityServerConfig.GetUsers())
    .AddJwtBearerClientAuthentication();
    //.AddAspNetIdentity<IdentityUser>()
    //.AddOperationalStore(options => options.ConfigureDbContext =
    //builder => builder.UseSqlServer(
    //    connectionString,
    //    sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseIdentityServer();
app.UseAuthorization();
app.UseRouting();

app.MapControllers();

app.Run();
