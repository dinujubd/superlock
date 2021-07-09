using SuperLocker.Core.Command;
using SuperLocker.Core.Dtos;
using System;
using System.Threading.Tasks;

namespace SuperLocker.Core.Repositories
{
    public interface ILockRepository
    {
        Task Unlock(UnlockCommand lockInfo);
        Task<Lock> GetLockAsync(Guid lockId);
        Task<UserLock> GetUserLockAsync(Guid userId, Guid lockId);
    }
}
