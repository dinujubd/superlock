using SuperLocker.Core.Command;
using SuperLocker.Core.Query;
using System.Threading.Tasks;

namespace SuperLocker.Core.Repositories
{
    public interface ILockRepository
    {
        Task Unlock(UnlockCommand lockInfo);
        Task<UnlockQueryRespose> GetUserUnlockActivity(UnlockActivityQuery query);
    }
}
