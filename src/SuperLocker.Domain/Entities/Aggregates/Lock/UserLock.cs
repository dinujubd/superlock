using System;

namespace SuperLocker.Domain.Entities.Aggregates.Lock
{
    public class UserLock : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid LockId { get; set; }
    }
}