using System;

namespace SuperLocker.Core.Query
{
    public class UnlockActivityQuery : IQuery
    {
        public Guid UserId { get; set; }
    }
}
