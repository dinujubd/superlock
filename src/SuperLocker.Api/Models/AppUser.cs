using System;

namespace SuperLocker.Api.Models
{
    public class AppUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}