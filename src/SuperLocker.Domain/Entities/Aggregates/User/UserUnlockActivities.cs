using System;

namespace SuperLocker.Domain.Entities.Aggregates.User
{
    public class UserUnlockActivities : BaseEntity
    {
        public string LockCode { get; set; }
        public Guid LockId { get; set; }
    }
}