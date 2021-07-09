using FluentValidation;
using SuperLocker.Core.Query;
using SuperLocker.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SuperLocker.Core.Validators.Queries
{
    public class UnlockActivityQueryValidator: AbstractValidator<UnlockActivityQuery>
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
