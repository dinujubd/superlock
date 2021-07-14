using System;

namespace SuperLocker.Application.Commands
{
    public class UnlockCommand : ICommand
    {
        public UnlockCommand(Guid lockId, Guid userId)
        {
            LockId = lockId;
            UserId = userId;
        }

        public Guid LockId { get; set; }
        public Guid UserId { get; set; }
    }
}