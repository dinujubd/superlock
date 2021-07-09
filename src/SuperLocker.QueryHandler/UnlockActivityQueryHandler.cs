using FluentValidation;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UnlockActivityQuery> _logger;
        public UnlockActivityQueryHandler(IUserRepository userRepository, IValidator<UnlockActivityQuery> validator, ILogger<UnlockActivityQuery> logger)
        {
            _userRepository = userRepository;
            _validator = validator;
            _logger = logger;
        }
        public async Task<UnlockQueryRespose> ExecuteAsync(UnlockActivityQuery query)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(query);
                if (!validationResult.IsValid)
                {
                    validationResult.Errors.ForEach(x =>
                    {
                        _logger.LogError("ACTIVITY.VALIDATION.ERROR {0} ->", x.ErrorMessage);
                    });

                    return null;
                }
                else
                {
                    return await _userRepository.GetUserUnlockActivity(query);
                }

            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "ACTIVITY.FAILED.EXCEPTION");
                throw;
            }
        }
    }
}
