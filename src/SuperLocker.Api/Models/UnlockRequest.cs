using System;

namespace SuperLocker.Api.Models
{
    public sealed class UnlockRequest
    {
        public Guid LockId { get; set; }
    }
}