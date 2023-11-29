using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

namespace Identity.Controllers
{
    [Route("/auth")]
    [ApiController]
    public class IdentityController : Controller
    {
        public IActionResult Get()
        {
            return Ok("Secret");
        }
        
        public IActionResult Login()
        {
            return View();
        }
        
        public IActionResult Login(LoginViewModel vm)
        {
            return View();
        }
    }
}
