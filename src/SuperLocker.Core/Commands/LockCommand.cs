using System;

namespace SuperLocker.Core.Command
{
    public class LockCommand : ICommand
    {
        public LockCommand(Guid lockId, Guid userId)
        {
            this.LockId = lockId;
            this.UserId = userId;

        }
        public Guid LockId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
