using Dapper;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SuperLocker.Core.Command;
using SuperLocker.Core.Dtos;
using SuperLocker.Core.Query;
using SuperLocker.Core.Repositories;
using SuperLocker.DataContext.Adapters;
using SuperLocker.DataContext.Providers;
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
        private readonly ICacheAdapter _cacheAdapter;
  
        public UserRepository(ConnectionPool<MySqlConnection> connectionPool, ILogger<UnlockCommand> logger, ICacheAdapter cacheAdapter)
        {
            _logger = logger;
            _connectionPool = connectionPool;
            _conn = connectionPool.Get();
            _cacheAdapter = cacheAdapter;
        }

        public async Task<UnlockQueryRespose> GetUserUnlockActivity(UnlockActivityQuery query)
        {
            var user = await GetUserAsync(query.UserId);

            if (user == null)
            {
                throw new InvalidOperationException("Userd lock has no match");
            }

            var lockResponse = await getUserUnlockActivites(query);

            _connectionPool.Return(_conn);

            return new UnlockQueryRespose
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                LastUnlocked = lockResponse.Select(x => x.unlock_time).ToList()
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

        private async Task<IEnumerable<UnlockTime>> getUserUnlockActivites(UnlockActivityQuery query)
        {
            var lockInfo = "SELECT unlock_time from AppDBSuperLock.unlock_activity_logs where user_id = @UserId";

            var lockResponse = await this._conn.QueryAsync<UnlockTime>(lockInfo, new { UserId = query.UserId.ToString() });
            return lockResponse;
        }

    }
}