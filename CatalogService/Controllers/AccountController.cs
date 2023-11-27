using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    /// <summary>
    /// Account
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IPasswordHasher<User> hasher;
        private readonly AppDbContext database;
        private readonly HttpContext ctx;
        public AccountController(IPasswordHasher<User> _hasher, AppDbContext _database, IHttpContextAccessor _ctxAccesor)
        {
            hasher = _hasher;
            database = _database;
            ctx = _ctxAccesor.HttpContext;
        }
        /// <summary>
        /// Register a user
        /// </summary>
        [HttpGet("register")]
        public async IAsyncEnumerable<User> Register(string username, string password)
        {
            var user = new User() { UserName = username };
            user.PasswordHash = hasher.HashPassword(user, password);
            await database.AddAsync(user);
            await ctx.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                UserHelper.Convert(user)
                );
            yield return user;
        }
    }
}
