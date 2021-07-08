using SuperLocker.Core;
using SuperLocker.Core.Query;
using SuperLocker.Core.Repositories;
using System.Threading.Tasks;

namespace SuperLocker.QueryHandler
{
    public class UnlockActivityQueryHandler : IQueryHandler<UnlockActivityQuery, UnlockQueryRespose>
    {
        private readonly IUserRepository _userRepository;
        public UnlockActivityQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<UnlockQueryRespose> ExecuteAsync(UnlockActivityQuery query)
        {
            return _userRepository.GetUserUnlockActivity(query);
        }
    }
}
