using SuperLocker.Core.Dtos;
using SuperLocker.Core.Query;
using System;
using System.Threading.Tasks;

namespace SuperLocker.Core.Repositories
{
    public interface IUserRepository
    {
        Task<UnlockQueryRespose> GetUserUnlockActivity(UnlockActivityQuery query);
        Task<User> GetUserAsync(Guid userId);
    }
}
