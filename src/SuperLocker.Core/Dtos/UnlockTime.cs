using System;

namespace SuperLocker.Core.Dtos
{
    public class UnlockData
    {
        public string LockId { get; set; }
        public string LockCode { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}