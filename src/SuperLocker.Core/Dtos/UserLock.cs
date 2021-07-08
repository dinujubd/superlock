using System;

namespace SuperLocker.Core.Dtos
{
    public class UserLock
    {
        public Guid Id
        {
            get
            {
                return new Guid(UserLockId);
            }
            private set
            {
                Id = value;
            }
        }
        public string UserLockId { get; set; }
        public string LockId { get; set; }
        public string UserId { get; set; }
    }

}