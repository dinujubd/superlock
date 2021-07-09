using Dapper;
using IdentityServer4.Validation;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace SuperLocker.Auth
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly string _connectionString = "Server=db;Database=AppDBSuperLock;Uid=root;Pwd=rpass;";
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            using (var _conn = new MySqlConnection(_connectionString))
            {
                var query = "select UserId, Secret from Users where UserName=@UserName AND IsActive=1 LIMIT 1";

                var user = await _conn.QueryFirstOrDefaultAsync<User>(query, new { UserName = context.UserName });

                if (user != null && BCrypt.Net.BCrypt.Verify(context.Password, user.Secret))
                {
                    context.Result = new GrantValidationResult(user.UserId, "password");
                }

            }
        }
    }

    public class User
    {
        public string UserId { get; set; }
        public string Secret { get; set; }
    }
}