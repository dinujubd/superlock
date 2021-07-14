using System.Threading.Tasks;
using Dapper;
using IdentityServer4.Validation;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using SuperLocker.Shared;

namespace SuperLocker.Auth
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IOptions<DatabaseConfigurations> _config;

        public ResourceOwnerPasswordValidator(IOptions<DatabaseConfigurations> config)
        {
            _config = config;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            using (var _conn = new MySqlConnection(_config.Value.ConnectionString))
            {
                var query = "select UserId, Secret from Users where UserName=@UserName AND IsActive=1 LIMIT 1";

                var user = await _conn.QueryFirstOrDefaultAsync<UserSecret>(query, new {context.UserName});

                if (user != null && BCrypt.Net.BCrypt.Verify(context.Password, user.Secret))
                    context.Result = new GrantValidationResult(user.UserId, "password");
            }
        }
    }
}