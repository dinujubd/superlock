using System;
using System.Collections.Generic;

namespace SuperLocker.Core.Query
{
    public class UnlockQueryRespose
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<DateTime> LastUnlocked  { get; set; }
    }
}