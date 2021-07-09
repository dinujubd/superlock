using FluentValidation;
using SuperLocker.Core;
using SuperLocker.Core.Query;
using SuperLocker.Core.Repositories;
using System.Threading.Tasks;

namespace SuperLocker.QueryHandler
{
    public class UnlockActivityQueryHandler : IQueryHandler<UnlockActivityQuery, UnlockQueryRespose>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UnlockActivityQuery> _validator;
        public UnlockActivityQueryHandler(IUserRepository userRepository, IValidator<UnlockActivityQuery> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }
        public async Task<UnlockQueryRespose> ExecuteAsync(UnlockActivityQuery query)
        {
            var validationResult = await _validator.ValidateAsync(query);
            if (validationResult.IsValid)
            {
                // Throw
            }

            return await _userRepository.GetUserUnlockActivity(query);
        }
    }
}
