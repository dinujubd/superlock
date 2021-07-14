using System;

namespace SuperLocker.Application.Queries
{
    public class UnlockActivityQuery : IQuery
    {
        public Guid UserId { get; set; }
    }
}