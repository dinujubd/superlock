using System;

namespace SuperLocker.Api.Models
{
    public sealed class LockCommadRequest
    {
        public Guid LockId { get; set; }
        public Guid UserId { get; set; }
    }
}