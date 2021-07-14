using System;
using System.Collections.Generic;
using SuperLocker.Application.Dtos;

namespace SuperLocker.Application.Queries
{
    public class UnlockQueryRespose
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<UnlockData> LastUnlocked { get; set; }
    }
}