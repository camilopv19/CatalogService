using BusinessLogicLayer.Identity;
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
        private readonly IUserService _userService;
        private readonly HttpContext ctx;
        public AccountController(IPasswordHasher<User> _hasher, IHttpContextAccessor _ctxAccesor, IUserService userService)
        {
            hasher = _hasher;
            ctx = _ctxAccesor.HttpContext;
            _userService = userService;
        }
        /// <summary>
        /// Register a user
        /// </summary>
        [HttpGet("register")]
        public async IAsyncEnumerable<User> Register(string username, string password, string role)
        {
            var user = new User() { UserName = username, Role = role };
            user.PasswordHash = hasher.HashPassword(user, password);
            _userService.AddAsync(user);
            await ctx.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                UserHelper.Convert(user)
                );
            yield return user;
        }
        /// <summary>
        /// User Login
        /// </summary>
        [HttpGet("login")]
        public async Task<string> Login(string username, string password)
        {
            var user = await _userService.GetByUsernameAsync(username);
            if (user != null)
            {
                var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
                if (result == PasswordVerificationResult.Failed)
                {
                    return "Bad credentials";
                }

                await ctx.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    UserHelper.Convert(user)
                    );
                return "Logged in";
            }
            return "Bad credentials";
        }
    }
}
