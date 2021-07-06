using System.Threading.Tasks;
using SuperLocker.Core.Command;

namespace SuperLocker.Core.Repositories
{
    public interface ILockRepository
    {
        Task Lock(LockCommand lockInfo);
    }
}
