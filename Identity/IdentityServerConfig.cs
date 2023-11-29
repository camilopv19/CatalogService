using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Identity
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientId="oauthClient",
                    ClientName = "Example client application using client credentials",
                    ClientSecrets =new List<Secret> {new Secret("client_secret".Sha256())},
                    AllowedGrantTypes= GrantTypes.ClientCredentials,
                    AllowedScopes= new List<string> { "api1.read", "api1" },
                }
            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
         {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            //new IdentityResources.Email(), //Not needed
            new IdentityResource
            {
                Name = "role",
                UserClaims = new List<string> {"role"}
            }
        };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
            new ApiResource
            {
                Name = "api1",
                DisplayName = "API #1",
                Description = "Allow the application to access API #1 on your behalf",
                Scopes = new List<string> {"api1.read", "api1.write"},
                ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())}, // change me!
                UserClaims = new List<string> {"role"}
            }
        };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
            new ApiScope("api1.read", "Read Access to API #1"),
            new ApiScope("api1.write", "Write Access to API #1")
        };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser> {
            new TestUser {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                Username = "bob",
                Password = "123",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Email, "bob@builder.com"),
                    new Claim(JwtClaimTypes.Role, "Manager")
                }
            }
        };
        }
    }
}
