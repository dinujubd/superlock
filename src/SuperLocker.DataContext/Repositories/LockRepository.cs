using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SuperLocker.Core.Command;
using SuperLocker.Core.Query;
using SuperLocker.Core.Repositories;

namespace SuperLocker.DataContext.Repositories
{
    public class LockRepository : ILockRepository, IDisposable
    {
        private readonly ILogger<UnlockCommand> _logger;
        private readonly MySqlConnection _conn;

        private string _connectionString = "Server=localhost;Database=AppDBSuperLock;Uid=root;Pwd=rpass;";

        public LockRepository(ILogger<UnlockCommand> logger)
        {
            _logger = logger;
            _conn = new MySqlConnection(_connectionString);
        }


        public async Task<UnlockQueryRespose> GetUserUnlockActivity(UnlockActivityQuery query)
        {
            var userInfo = "SELECT * from AppDBSuperLock.users where user_id = @UserId";

            var user = await this._conn.QueryFirstOrDefaultAsync<User>(userInfo, new { UserId = query.UserId });

            if (user == null) throw new InvalidOperationException("Userd lock has no match");

            var lockInfo = "SELECT unlock_time from AppDBSuperLock.user_locks where user_id = @UserId";

            var lockResponse = await this._conn.QueryAsync<UnlockTime>(lockInfo, new { UserId = query.UserId.ToString() });

            return new UnlockQueryRespose
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
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



        public void Dispose()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!isDisposing)
                _conn.Close();
        }
    }

    public class UserLock
    {
        public Guid Id
        {
            get
            {
                return new Guid(user_lock_id);
            }
            private set
            {
                Id = value;
            }
        }
        public string user_lock_id { get; set; }
        public string lock_id { get; set; }
        public string user_id { get; set; }
    }


    public class UnlockTime
    {
        public DateTime unlock_time { get; set; }
    }
    public class User
    {
        public Guid Id
        {
            get
            {
                return new Guid(user_id);
            }
            private set
            {
                Id = value;
            }
        }
        public string user_id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }



}