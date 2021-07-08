using System;

namespace SuperLocker.Core.Dtos
{
    public class Lock
    {
        public Guid Id
        {
            get
            {
                return Guid.Parse(LockId);
            }
            private set => Id = value;
        }
        public string LockId { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
    }
}
