using System;
using System.Threading.Tasks;

namespace SuperLocker.Domain.Entities.Aggregates.Lock.Repository
{
    public interface ILockRepository
    {
        Task Unlock(Guid lockId, Guid userId);
        Task<Lock> GetLockAsync(Guid lockId);
        Task<UserLock> GetUserLockAsync(Guid userId, Guid lockId);
    }
}