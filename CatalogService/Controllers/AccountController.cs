﻿using BusinessLogicLayer.Identity;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;

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
        private readonly IHttpClientFactory _httpClientFactory;
        public AccountController(IPasswordHasher<User> _hasher, IHttpContextAccessor _ctxAccesor, IUserService userService, IHttpClientFactory httpClientFactory)
        {
            hasher = _hasher;
            ctx = _ctxAccesor.HttpContext;
            _userService = userService;
            _httpClientFactory = httpClientFactory;
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
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userService.GetByUsernameAsync(username);
            if (user != null)
            {
                var secretKey = "MyAnonymousAndSecuredSecretKey";
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

                var jwt = new JwtSecurityToken(
                    claims: UserHelper.BuildClaims(user),
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512)
                    );

                var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                return Ok(token);
            }
            return Unauthorized("Bad credentials");
        }

        /// <summary>
        /// Get access token
        /// </summary>
        [HttpGet("GetToken")]
        public async Task<IActionResult> GetToken()
        {
            // Retrieve access token
            var srvrClient = _httpClientFactory.CreateClient();
            var discoveryDoc = await srvrClient.GetDiscoveryDocumentAsync("https://localhost:7297/");
            var tokenResponse = await srvrClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDoc.TokenEndpoint,
                    ClientId = "oauthClient",
                    ClientSecret = "client_secret",
                    Scope = "api1.read",
                });

            // Retrieve secret data
            var apiClient = _httpClientFactory.CreateClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = await apiClient.GetAsync("https://localhost:7297/secret");
            var content = await response.Content.ReadAsStringAsync();

            return Ok(new
            {
                token = tokenResponse.AccessToken,
                message = content
            });
        }
    }
}
