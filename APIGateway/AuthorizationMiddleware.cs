using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
namespace APIGateway
{

    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.ToString();
            if (path.Contains("connect") || path.Contains("login"))
            {
                await _next(context);
            }
            else
            {
                var authorizationHeaders = context.Request.Headers["Authorization"];
                var user = context.User;
                // Check if the user has the "admin" role
                if (user.IsInRole("Manager") || user.IsInRole("Buyer"))
                {
                    // User is authorized, continue with the request
                    await _next(context);
                }
                else
                {
                    // User is not authorized, return a 403 Forbidden response
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("You are not authorized to access this resource.");
                }

            }
        }
    }
}
