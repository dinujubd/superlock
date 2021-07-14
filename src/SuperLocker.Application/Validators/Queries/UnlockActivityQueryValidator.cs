using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using SuperLocker.Application.Queries;
using SuperLocker.Domain.Entities.Aggregates.User.Repository;

namespace SuperLocker.Application.Validators.Queries
{
    public class UnlockActivityQueryValidator : AbstractValidator<UnlockActivityQuery>
    {
        private readonly IUserRepository _userRepository;

        public UnlockActivityQueryValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            RuleFor(x => x.UserId).MustAsync(ActiveUserExists).WithMessage("User Not Found");
        }

        private async Task<bool> ActiveUserExists(Guid userId, CancellationToken arg2)
        {
            var userLock = await _userRepository.GetUserAsync(userId);
            return userLock != null;
        }
    }
}