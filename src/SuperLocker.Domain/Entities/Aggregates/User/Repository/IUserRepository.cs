using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperLocker.Domain.Entities.Aggregates.User.Repository
{
    public interface IUserRepository
    {
        Task<IList<UserUnlockActivities>> GetUserUnlockActivity(Guid userId);
        Task<User> GetUserAsync(Guid userId);
    }
}