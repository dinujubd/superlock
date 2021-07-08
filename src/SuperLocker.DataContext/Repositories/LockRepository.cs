using Dapper;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SuperLocker.Core.Command;
using SuperLocker.Core.Query;
using SuperLocker.Core.Repositories;
using SuperLocker.DataContext.Adapters;
using SuperLocker.DataContext.Dtos;
using SuperLocker.DataContext.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperLocker.DataContext.Repositories
{
    public class LockRepository : ILockRepository
    {
        private readonly ILogger<UnlockCommand> _logger;
        private readonly MySqlConnection _conn;
        private readonly ConnectionPool<MySqlConnection> _connectionPool;
        private readonly ICacheAdapter _cacheAdapter;

        public LockRepository(ConnectionPool<MySqlConnection> connectionPool, ILogger<UnlockCommand> logger, ICacheAdapter cacheAdapter)
        {
            _logger = logger;
            _connectionPool = connectionPool;
            _conn = connectionPool.Get();
            _cacheAdapter = cacheAdapter;
        }

        public async Task<UnlockQueryRespose> GetUserUnlockActivity(UnlockActivityQuery query)
        {
            var user = await GetUserAsync(query);

            if (user == null)
            {
                throw new InvalidOperationException("Userd lock has no match");
            }

            var lockResponse = await getUserUnlockActivites(query);

            _connectionPool.Return(_conn);

            return new UnlockQueryRespose
            {
                UserId = user.Id,
                FirstName = user.first_name,
                LastName = user.last_name,
                LastUnlocked = lockResponse.Select(x => x.unlock_time).ToList()
            };
        }

        public async Task Unlock(UnlockCommand lockInfo)
        {
            var result = await GetUserLock(lockInfo);

            if (result == null)
            {
                throw new InvalidOperationException("Userd lock has no match");
            }

            await ApplyUnlock(lockInfo);
        }

        private async Task<IEnumerable<UnlockTime>> getUserUnlockActivites(UnlockActivityQuery query)
        {
            var lockInfo = "SELECT unlock_time from AppDBSuperLock.unlock_activity_logs where user_id = @UserId";

            var lockResponse = await this._conn.QueryAsync<UnlockTime>(lockInfo, new { UserId = query.UserId.ToString() });
            return lockResponse;
        }

        private async Task<User> GetUserAsync(UnlockActivityQuery query)
        {
            return await _cacheAdapter.QueryWithCache($"get_user_{query.UserId}", async () =>
            {
                var userInfo = "SELECT * from Users where UserId = @UserId And IsActive=1 LIMIT 1";
                return await this._conn.QueryFirstOrDefaultAsync<User>(userInfo, new { UserId = query.UserId });
            });
        }


        private async Task<UserLock> GetUserLock(UnlockCommand lockInfo)
        {
            return await _cacheAdapter.QueryWithCache($"get_user_lock_{lockInfo.UserId}_{lockInfo.LockId}", async () =>
            {
                var query = "SELECT * from AppDBSuperLock.user_locks where lock_id = @LockId and user_id = @UserId LIMIT 1";
                return await this._conn.QueryFirstOrDefaultAsync<UserLock>(query, new { LockId = lockInfo.LockId.ToString(), UserId = lockInfo.UserId.ToString() });
            });
        }

        private async Task ApplyUnlock(UnlockCommand lockInfo)
        {
            var unlockQuery = @"INSERT into AppDBSuperLock.unlock_activity_logs (unlock_activity_id, lock_id, user_id, unlock_time)
                                values (@Id, @LockId, @UserId, @UnlockTime);
                                ";
            await this._conn.ExecuteAsync(unlockQuery, new
            {
                Id = Guid.NewGuid().ToString(),
                LockId = lockInfo.LockId.ToString(),
                UserId = lockInfo.UserId.ToString(),
                UnlockTime = DateTime.UtcNow
            });
        }

    }

}