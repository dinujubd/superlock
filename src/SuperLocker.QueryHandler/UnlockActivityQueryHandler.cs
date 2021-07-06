using System;
using System.Threading.Tasks;
using SuperLocker.Core;
using SuperLocker.Core.Query;
using SuperLocker.Core.Repositories;

namespace SuperLocker.QueryHandler
{
    public class UnlockActivityQueryHandler : IQueryHandler<UnlockActivityQuery, UnlockQueryRespose>
    {
        private readonly ILockRepository _lockRepository;
        public UnlockActivityQueryHandler(ILockRepository lockRepository)
        {
            _lockRepository = lockRepository;
        }
        public Task<UnlockQueryRespose> ExecuteAsync(UnlockActivityQuery query)
        {
            return _lockRepository.GetUserUnlockActivity(query);
        }
    }
}
