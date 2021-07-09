using Dapper;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using SuperLocker.Crosscuts;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SuperLocker.Auth
{
    public class ProfileSerice : IProfileService
    {
        private readonly IOptions<DatabaseConfigurations> _config;
        public ProfileSerice(IOptions<DatabaseConfigurations> config)
        {
           _config = config;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var _connectionString = _config.Value.ConnectionString;

            using (var _conn = new MySqlConnection(_connectionString))
            {
                var userId = context.Subject.Claims.Where(x => x.Type == "sub").Select(x => x.Value).FirstOrDefault();

                if (userId != null)
                {
                    var query = @"SELECT u.UserName, u.FirstName, u.LastName, r.Name AS RoleName FROM AppDBSuperLock.UserRoles ur
                                INNER JOIN AppDBSuperLock.Roles r
                                ON r.RoleId = ur.RoleId
                                INNER JOIN AppDBSuperLock.Users u
                                ON u.UserId = ur.UserId
                                WHERE ur.UserId = @UserId";

                    var roles = await _conn.QueryAsync<UserRole>(query, new { UserId = userId });

                    var singleUser = roles.FirstOrDefault();

                    if (singleUser != null)
                    {
                        context.IssuedClaims.AddRange(roles.Select(x => new Claim("role", x.RoleName)));
                        context.IssuedClaims.Add(new Claim("userId", userId));
                        context.IssuedClaims.Add(new Claim("firstName", singleUser.FirstName));
                        context.IssuedClaims.Add(new Claim("lastName", singleUser.LastName));
                        context.IssuedClaims.Add(new Claim("userName", singleUser.UserName));
                    }
                }
                context.IssuedClaims.AddRange(context.Subject.Claims);
            }

        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }

    public class UserRole
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
    }
}