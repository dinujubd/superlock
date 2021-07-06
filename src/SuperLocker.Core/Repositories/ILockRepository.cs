using System.Threading.Tasks;
using SuperLocker.Core.Command;
using SuperLocker.Core.Query;

namespace SuperLocker.Core.Repositories
{
    public interface ILockRepository
    {
        Task Unlock(UnlockCommand lockInfo);
        Task<UnlockQueryRespose> GetUserUnlockActivity(UnlockActivityQuery query);
    }
}
