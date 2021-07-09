using Dapper;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SuperLocker.Core.Command;
using SuperLocker.Core.Dtos;
using SuperLocker.Core.Repositories;
using SuperLocker.DataContext.Adapters;
using SuperLocker.DataContext.Providers;
using SuperLocker.DataContext.Proxies;
using System;
using System.Threading.Tasks;

namespace SuperLocker.DataContext.Repositories
{
    public class LockRepository : ILockRepository
    {
        private readonly ILogger<UnlockCommand> _logger;
        private readonly MySqlConnection _conn;
        private readonly ICacheProxy _cacheProxy;
        private readonly ICacheAdapter _cacheAdapter;

        public LockRepository(ConnectionPool<MySqlConnection> connectionPool, ILogger<UnlockCommand> logger, ICacheProxy cacheProxy, ICacheAdapter cacheAdapter)
        {
            _logger = logger;
            _conn = connectionPool.Get();
            _cacheProxy = cacheProxy;
            _cacheAdapter = cacheAdapter;
        }

        public async Task Unlock(UnlockCommand lockInfo)
        {
            await ApplyUnlock(await GetUserLockAsync(lockInfo.UserId, lockInfo.LockId));
        }


        public async Task<Lock> GetLockAsync(Guid lockId)
        {
            return await _cacheProxy.QueryWithCache($"get_lock_{lockId}", async () =>
            {
                var userInfo = "SELECT * from Locks where LockId = @LockId And IsActive=1 LIMIT 1";
                return await this._conn.QueryFirstOrDefaultAsync<Lock>(userInfo, new { LockId = lockId });
            });
        }

        public async Task<UserLock> GetUserLockAsync(Guid userId, Guid lockId)
        {
            return await _cacheProxy.QueryWithCache($"get_user_lock_{userId}_{lockId}", async () =>
            {
                var query = "SELECT * from UserLocks where LockId = @LockId and UserId = @UserId LIMIT 1";
                return await _conn.QueryFirstOrDefaultAsync<UserLock>(query, new { LockId = lockId.ToString(), UserId = userId.ToString() });
            });
        }

        private async Task ApplyUnlock(UserLock userLock)
        {
            var unlockQuery = @"INSERT into UserUnlockActivity (UserUnlockActivityId, UserId, LockId, UserLockId, CreatedOn)
                                values (@UserUnlockActivityId, @UserId, @LockId, @UserLockId, @CreatedOn);";

            var inseted = await this._conn.ExecuteAsync(unlockQuery, new
            {
                UserUnlockActivityId = Guid.NewGuid().ToString(),
                LockId = userLock.LockId,
                UserId = userLock.UserId,
                UserLockId = userLock.UserLockId,
                CreatedOn = DateTime.Now
            });

            if (inseted > 0)
            {
                var lockInfo = await GetLockAsync(Guid.Parse(userLock.LockId));

                await _cacheAdapter.PushFixed($"get_{userLock.UserId}_last_unlocks",
                    new UnlockData { LockId = userLock.LockId, CreatedOn = DateTime.UtcNow, LockCode = lockInfo.Code });
            }
        }

    }
}