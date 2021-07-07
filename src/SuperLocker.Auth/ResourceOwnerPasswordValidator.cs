using System.Threading.Tasks;
using IdentityServer4.Validation;
using MySql.Data.MySqlClient;
using Dapper;
using static IdentityModel.OidcConstants;
using System;

namespace SuperLocker.Auth
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private string _connectionString = "Server=localhost;Database=AppDBSuperLock;Uid=root;Pwd=rpass;";
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            using (var _conn = new MySqlConnection(_connectionString))
            {
                var query = "select user_id, user_secret from users where user_name=@UserName AND is_active=1 LIMIT 1";

                var user = await _conn.QueryFirstOrDefaultAsync<User>(query, new {UserName = context.UserName });

                if(user != null && BCrypt.Net.BCrypt.Verify(context.Password, user.user_secret)) {

                    context.Result = new GrantValidationResult(user.user_id, "password");
                }   
                
            }
        }
    }

    public class User {
        public string user_id { get; set; }
        public string user_secret { get; set; }
    }
}