using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SuperLocker.Core.Command;
using SuperLocker.Core.Query;
using SuperLocker.Core.Repositories;
using SuperLocker.DataContext.Dtos;
using SuperLocker.DataContext.Providers;

namespace SuperLocker.DataContext.Repositories
{
    public class LockRepository : ILockRepository
    {
        private readonly ILogger<UnlockCommand> _logger;
        private readonly MySqlConnection _conn;
        private readonly ConnectionPool<MySqlConnection> _connectionPool;

    
        public LockRepository(ConnectionPool<MySqlConnection> connectionPool, ILogger<UnlockCommand> logger)
        {
            _logger = logger;
            _connectionPool = connectionPool;
            _conn = connectionPool.Get();
        }

        public async Task<UnlockQueryRespose> GetUserUnlockActivity(UnlockActivityQuery query)
        {
            var userInfo = "SELECT * from AppDBSuperLock.users where user_id = @UserId";

            var user = await this._conn.QueryFirstOrDefaultAsync<User>(userInfo, new { UserId = query.UserId });

            if (user == null) throw new InvalidOperationException("Userd lock has no match");

            var lockInfo = "SELECT unlock_time from AppDBSuperLock.unlock_activity_logs where user_id = @UserId";

            var lockResponse = await this._conn.QueryAsync<UnlockTime>(lockInfo, new { UserId = query.UserId.ToString() });

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
            var sql = "SELECT * from AppDBSuperLock.user_locks where lock_id = @LockId and user_id = @UserId";

            var result = await this._conn.QueryFirstOrDefaultAsync<UserLock>(sql, new { LockId = lockInfo.LockId.ToString(), UserId = lockInfo.UserId.ToString() });

            if (result == null)
            {
                throw new InvalidOperationException("Userd lock has no match");
            }

            await ApplyUnlock(lockInfo);
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