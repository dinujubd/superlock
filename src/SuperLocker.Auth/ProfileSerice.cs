using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;

namespace SuperLocker.Auth
{
    public class ProfileSerice : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims.AddRange(context.Subject.Claims);

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}