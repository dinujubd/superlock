using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using SuperLocker.Domain.Entities.Aggregates.User;
using SuperLocker.Domain.Entities.Aggregates.User.Repository;
using SuperLocker.Infrastructure.Providers;
using SuperLocker.Infrastructure.Proxies;

namespace SuperLocker.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ICacheProxy _cacheAdapter;
        private readonly MySqlConnection _conn;
        private readonly ConnectionPool<MySqlConnection> _connectionPool;


        public UserRepository(ConnectionPool<MySqlConnection> connectionPool,
            ICacheProxy cacheAdapter)
        {
            _connectionPool = connectionPool;
            _conn = connectionPool.Get();
            _cacheAdapter = cacheAdapter;
        }

        public async Task<IList<UserUnlockActivities>> GetUserUnlockActivity(Guid userId)
        {
            var lockResponse = await GetUserUnlockActivities(userId);

            _connectionPool.Return(_conn);

            return lockResponse;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _cacheAdapter.QueryWithCache($"get_user_{userId}", async () =>
            {
                const string userInfo =
                    "SELECT UserId as Id, FirstName, LastName, UserName, PhoneNo, IsActive from Users where UserId = @UserId And IsActive=1 LIMIT 1";
                return await _conn.QueryFirstOrDefaultAsync<User>(userInfo, new {UserId = userId.ToString()});
            });
        }

        private async Task<IList<UserUnlockActivities>> GetUserUnlockActivities(Guid userId)
        {
            return await _cacheAdapter.QueryWithCache($"get_{userId}_last_unlocks", async () =>
            {
                var lockInfo = @"SELECT a.UserUnlockActivityId as Id, l.LockId, l.Code AS LockCode, a.CreatedOn 
                            FROM UserUnlockActivity AS a
                            INNER JOIN Locks AS l
                            ON l.LockId = a.LockId
                            WHERE l.IsActive AND UserId = @UserId
                            ORDER BY a.CreatedOn LIMIT 20";

                return (await _conn.QueryAsync<UserUnlockActivities>(lockInfo, new {UserId = userId.ToString()}))
                    .ToList();
            });
        }
    }
}