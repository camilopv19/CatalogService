using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    [Keyless]
    public class UserHash
    {
        public string Hash { get; set; }
    }

    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public List<UserClaim> Claims { get; set; } = new();
    }
    public class UserClaim
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class UserHelper
    {
        public static ClaimsPrincipal Convert(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim("username", user.UserName)
            };

            claims.AddRange(user.Claims.Select(c => new Claim(c.Type, c.Value)));
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }
    }
}
