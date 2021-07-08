using System;

namespace SuperLocker.Core.Command
{
    public class UnlockCommand : ICommand
    {
        public UnlockCommand(Guid lockId, Guid userId)
        {
            this.LockId = lockId;
            this.UserId = userId;

        }
        public Guid LockId { get; set; }
        public Guid UserId { get; set; }
    }
}
