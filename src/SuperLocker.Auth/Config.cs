using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace SuperLocker.Auth
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1", "openid", "profile", "email"}
                }
            };

        public static List<TestUser> TestUsers =>
            new List<TestUser> {
                new TestUser {
                    SubjectId = "user_1",
                    Username = "alice",
                    Password = "alice",
                    Claims = new List<Claim> {
                        new Claim("firstname", "Alice"),
                        new Claim("lastname", "Bobo"),
                        new Claim(JwtClaimTypes.Role, "admin"),
                    }
                },
                new TestUser {
                    SubjectId = "user_1",
                    Username = "bob",
                    Password = "bob",
                    Claims = new List<Claim> {
                        new Claim("firstname", "Alice"),
                        new Claim("lastname", "Bobo"),
                        new Claim(JwtClaimTypes.Role, "user"),
                    }
                }
            };

        public static IEnumerable<IdentityResource> Resouces =>
            new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new ProfileWithRoleIdentityResource(),
                new IdentityResources.Email()
            };
    }


    public class ProfileWithRoleIdentityResource: IdentityResources.Profile
    {
        public ProfileWithRoleIdentityResource()
        {
            this.UserClaims.Add(JwtClaimTypes.Role);
        }
    }
}