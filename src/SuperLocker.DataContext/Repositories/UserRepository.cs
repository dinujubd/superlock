using Dapper;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SuperLocker.Core.Command;
using SuperLocker.Core.Dtos;
using SuperLocker.Core.Query;
using SuperLocker.Core.Repositories;
using SuperLocker.DataContext.Providers;
using SuperLocker.DataContext.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperLocker.DataContext.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UnlockCommand> _logger;
        private readonly MySqlConnection _conn;
        private readonly ConnectionPool<MySqlConnection> _connectionPool;
        private readonly ICacheProxy _cacheAdapter;

        public UserRepository(ConnectionPool<MySqlConnection> connectionPool, ILogger<UnlockCommand> logger, ICacheProxy cacheAdapter)
        {
            _logger = logger;
            _connectionPool = connectionPool;
            _conn = connectionPool.Get();
            _cacheAdapter = cacheAdapter;
        }

        public async Task<UnlockQueryRespose> GetUserUnlockActivity(UnlockActivityQuery query)
        {
            var user = await GetUserAsync(query.UserId);

            var lockResponse = await getUserUnlockActivites(query);

            _connectionPool.Return(_conn);

            return new UnlockQueryRespose
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                LastUnlocked = lockResponse.ToList()
            };
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _cacheAdapter.QueryWithCache($"get_user_{userId}", async () =>
            {
                var userInfo = "SELECT * from Users where UserId = @UserId And IsActive=1 LIMIT 1";
                return await this._conn.QueryFirstOrDefaultAsync<User>(userInfo, new { UserId = userId });
            });
        }

        private async Task<IEnumerable<UnlockData>> getUserUnlockActivites(UnlockActivityQuery query)
        {
            return await _cacheAdapter.QueryWithCache($"get_{query.UserId}_last_unlocks", async () =>
            {
                var lockInfo = @"SELECT l.LockId, l.Code AS LockCode, a.CreatedOn 
                            FROM UserUnlockActivity AS a
                            INNER JOIN Locks AS l
                            ON l.LockId = a.LockId
                            WHERE l.IsActive AND UserId = @UserId
                            ORDER BY a.CreatedOn LIMIT 20";

                return await this._conn.QueryAsync<UnlockData>(lockInfo, new { UserId = query.UserId.ToString() });
            });
        }
    }
}