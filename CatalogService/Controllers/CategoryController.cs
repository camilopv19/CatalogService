using BusinessLogicLayer.CoreLogic;
using DataAccessLayer.Entities;
using IdentityModel;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CatalogService.Controllers
{
    /// <Summary>
    /// Categories API.
    /// </Summary>
    [Route("api/Categories")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly HttpContext ctx;
        private readonly TelemetryClient telemetryClient;

#pragma warning disable 1591
        public CategoryController(ICategoryService categoryService, IHttpContextAccessor _ctxAccesor, TelemetryClient telemetryClient)
        {
            _categoryService = categoryService;
#pragma warning disable CS8601 // Possible null reference assignment.
            ctx = _ctxAccesor.HttpContext;
#pragma warning restore CS8601 // Possible null reference assignment.
            this.telemetryClient = telemetryClient;
        }

        /// <summary>
        /// Get a list of Categories. Allowed roles: Manager, Buyer
        /// </summary>
        /// <returns>A list of Categories.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
        [Authorize(Roles = "Manager, Buyer")]
        public ActionResult<Category> Get()
        {
            var authorizationHeaders = HttpContext.Request.Headers["Authorization"];
            //var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var roleClaim = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (authorizationHeaders.Count > 0)
            {
                var headers = authorizationHeaders.ToString();
                // Validate the authorization header and access token
                if (!string.IsNullOrEmpty(headers) && headers.StartsWith("Bearer "))
                {
                    var token = headers.Substring("Bearer ".Length).Trim();

                    // Perform token validation logic (e.g., validate the signature, check expiration)
                    if (ValidateToken(token))
                    {
                        // Track a custom event
                        telemetryClient.TrackEvent("GetAllCategories");
                        return Ok(_categoryService.List());
                    }
                }
            }
            ctx.Response.StatusCode = 401; // Unauthorized
            return Unauthorized();
        }


        /// <summary>
        /// Finds a Category by its Id. Allowed roles: Manager, Buyer
        /// </summary>
        /// <param name="id">Number</param>
        /// <returns>An Categories</returns>
        [HttpGet("{id}", Name = "GetCategory")]
        [Authorize(Roles = "Manager, Buyer")]
        public ActionResult<Category> Get(int id)
        {
            var result = _categoryService.Get(id);
            if (result != default)
            {
                telemetryClient.TrackEvent("GetCategoryById");
                return Ok(_categoryService.Get(id));
            }
            else
                return NotFound();
        }

        /// <summary>
        /// Inserts a Category. Only Manager
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult<Category> Insert(Category dto)
        {
            var result = _categoryService.Upsert(dto);
            if (result != 0)
            {
                telemetryClient.TrackEvent("InsertCategory");
                return CreatedAtAction("Insert", _categoryService.Get(dto.Id));
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Updates a Category. Only Manager
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpPut]
        [Authorize(Roles = "Manager")]
        public ActionResult<Category> Update(Category dto)
        {
            var result = _categoryService.Upsert(dto);
            if (result > 0)
            {
                telemetryClient.TrackEvent("UpdateCategory");
                return NoContent();
            }
            else
                return NotFound();
        }

        /// <summary>
        /// Deletes a Category by its Id. Only Manager
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public ActionResult<Category> Delete(int id)
        {
            var result = _categoryService.Delete(id);
            if (result > 0)
            {
                telemetryClient.TrackEvent("DeleteCategory");
                return NoContent();
            }
            else
                return NotFound();
        }

        private bool ValidateToken(string token)
        {
            // Implement your token validation logic
            // Example using JwtSecurityTokenHandler (make sure to install the necessary NuGet package)

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                // Set your token validation parameters (issuer, audience, signing key, etc.)
                // Example:
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("MyAnonymousAndSecuredSecretKey"))
            };

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                return true; // Token is valid
            }
            catch (Exception ex)
            {
                // Token validation failed
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return false;
            }
        }
    }
}
