using System;

namespace SuperLocker.Core.Command
{
    public class LockCommand:ICommand
    {
        public Guid LockId { get; set; }
        public Guid UserId { get; set; }
    }
}
