using System;

namespace SuperLocker.Core.Dtos
{
    public class User
    {
        public Guid Id
        {
            get
            {
                return new Guid(user_id);
            }
            private set
            {
                Id = value;
            }
        }
        public string user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

}