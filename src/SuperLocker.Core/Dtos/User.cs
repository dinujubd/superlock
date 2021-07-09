using System;

namespace SuperLocker.Core.Dtos
{
    public class User
    {
        public Guid Id
        {
            get
            {
                return new Guid(UserId);
            }
            private set
            {
                Id = value;
            }
        }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}