using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Identity.svr
{
    public class Config
    {

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId="1",
                    Username="Kaushik1",
                    Password="password"
                },
                 new TestUser
                {
                    SubjectId="2",
                    Username="Kaushik2",
                    Password="password"
                },
            };
        }
        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("bankOfDotNetApi", "Customer Api for BankOfDotNet",new []{JwtClaimTypes.Subject, "TestClient", JwtClaimTypes.PhoneNumber })
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "bankOfDotNetApi" },
                     Claims=new List<Claim>()
                   {
                        new Claim(ClaimTypes.Country,"Ireland"),
                        new Claim(ClaimTypes.Actor,"Nithyananda"),
                        new Claim("City","Bidadi")
                   }
                },
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "bankOfDotNetApi" },
                    Claims=new List<Claim>()
                   {
                        new Claim(ClaimTypes.Country,"Ireland"),
                        new Claim(ClaimTypes.Actor,"Nithyananda"),
                        new Claim("City","Bidadi")
                   }
                }
            };
        
        }
    }
}
