using System;

namespace SuperLocker.DataContext.Dtos
{
    public class UserLock
    {
        public Guid Id
        {
            get
            {
                return new Guid(user_lock_id);
            }
            private set
            {
                Id = value;
            }
        }
        public string user_lock_id { get; set; }
        public string lock_id { get; set; }
        public string user_id { get; set; }
    }

}