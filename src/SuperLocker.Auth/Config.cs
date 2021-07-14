using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

namespace SuperLocker.Auth
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new("super_lock_api", "My API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new()
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"super_lock_api", "openid", "profile", "email"}
                }
            };

        public static IEnumerable<IdentityResource> Resouces =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new ProfileWithRoleIdentityResource(),
                new IdentityResources.Email()
            };
    }


    public class ProfileWithRoleIdentityResource : IdentityResources.Profile
    {
        public ProfileWithRoleIdentityResource()
        {
            UserClaims.Add(JwtClaimTypes.Role);
        }
    }
}