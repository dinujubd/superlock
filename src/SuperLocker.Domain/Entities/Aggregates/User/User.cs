using System.Collections.Generic;

namespace SuperLocker.Domain.Entities.Aggregates.User
{
    public class User : BaseEntity, IAggregateRoot
    {
        public IList<UserUnlockActivities> Activities { get; set; }
    }
}