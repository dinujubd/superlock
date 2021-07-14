using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SuperLocker.Application;
using SuperLocker.Application.Queries;
using SuperLocker.Domain.Entities.Aggregates.User;
using SuperLocker.Domain.Entities.Aggregates.User.Repository;

namespace SuperLocker.QueryHandler
{
    public class UnlockActivityQueryHandler : IQueryHandler<UnlockActivityQuery, IList<UserUnlockActivities>>
    {
        private readonly ILogger<UnlockActivityQuery> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UnlockActivityQuery> _validator;

        public UnlockActivityQueryHandler(IUserRepository userRepository, IValidator<UnlockActivityQuery> validator,
            ILogger<UnlockActivityQuery> logger)
        {
            _userRepository = userRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<QueryResponse<IList<UserUnlockActivities>>> ExecuteAsync(UnlockActivityQuery query)
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

                    return new QueryResponse<IList<UserUnlockActivities>>
                    {
                        Errors = new List<string> {"Invalid Request"}
                    };
                }

                var activities = await _userRepository.GetUserUnlockActivity(query.UserId);
                return new QueryResponse<IList<UserUnlockActivities>>
                {
                    Response = activities
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ACTIVITY.FAILED.EXCEPTION");
                throw;
            }
        }
    }
}