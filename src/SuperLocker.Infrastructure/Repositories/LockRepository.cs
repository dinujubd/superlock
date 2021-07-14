using System;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using SuperLocker.Application.Dtos;
using SuperLocker.Domain.Entities.Aggregates.Lock;
using SuperLocker.Domain.Entities.Aggregates.Lock.Repository;
using SuperLocker.Infrastructure.Adapters;
using SuperLocker.Infrastructure.Providers;
using SuperLocker.Infrastructure.Proxies;

namespace SuperLocker.Infrastructure.Repositories
{
    public class LockRepository : ILockRepository
    {
        private readonly ICacheAdapter _cacheAdapter;
        private readonly ICacheProxy _cacheProxy;
        private readonly MySqlConnection _conn;

        public LockRepository(ConnectionPool<MySqlConnection> connectionPool,
            ICacheProxy cacheProxy, ICacheAdapter cacheAdapter)
        {
            _conn = connectionPool.Get();
            _cacheProxy = cacheProxy;
            _cacheAdapter = cacheAdapter;
        }

        public async Task Unlock(Guid userId, Guid lockId)
        {
            await ApplyUnlock(await GetUserLockAsync(userId, lockId));
        }


        public async Task<Lock> GetLockAsync(Guid lockId)
        {
            return await _cacheProxy.QueryWithCache($"get_lock_{lockId}", async () =>
            {
                var userInfo =
                    "SELECT LockId as Id,Code, IsActive, CreatedOn, CreatedBy, UpdatedDate, UpdatedBy from Locks where LockId = @LockId And IsActive=1 LIMIT 1";
                return await _conn.QueryFirstOrDefaultAsync<Lock>(userInfo, new {LockId = lockId});
            });
        }

        public async Task<UserLock> GetUserLockAsync(Guid userId, Guid lockId)
        {
            return await _cacheProxy.QueryWithCache($"get_user_lock_{userId}_{lockId}", async () =>
            {
                var query =
                    "SELECT UserLockId as Id, UserId, LockId, CreatedOn, CreatedBy, UpdatedDate, UpdatedBy from UserLocks where LockId = @LockId and UserId = @UserId LIMIT 1";
                return await _conn.QueryFirstOrDefaultAsync<UserLock>(query,
                    new {LockId = lockId.ToString(), UserId = userId.ToString()});
            });
        }

        private async Task ApplyUnlock(UserLock userLock)
        {
            var unlockQuery =
                @"INSERT into UserUnlockActivity (UserUnlockActivityId, UserId, LockId, UserLockId, CreatedOn)
                                values (@UserUnlockActivityId, @UserId, @LockId, @UserLockId, @CreatedOn);";

            var inserted = await _conn.ExecuteAsync(unlockQuery, new
            {
                UserUnlockActivityId = Guid.NewGuid().ToString(),
                userLock.UserId,
                userLock.LockId,
                UserLockId = userLock.Id.ToString(),
                CreatedOn = DateTime.Now
            });

            if (inserted > 0)
            {
                var lockInfo = await GetLockAsync(userLock.LockId);

                await _cacheAdapter.PushFixed($"get_{userLock.UserId}_last_unlocks",
                    new UnlockData
                        {LockId = userLock.LockId.ToString(), CreatedOn = DateTime.UtcNow, LockCode = lockInfo.Code});
            }
        }
    }
}