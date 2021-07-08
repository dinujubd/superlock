using FluentValidation;
using SuperLocker.Core.Command;
using SuperLocker.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SuperLocker.Core.Validators.Command
{
    public class UnlockCommandValidator : AbstractValidator<UnlockCommand>
    {

        private readonly ILockRepository _lockRepository;
        private readonly IUserRepository _userRepository;
        public UnlockCommandValidator(ILockRepository lockRepository, IUserRepository userRepository)
        {
            _lockRepository = lockRepository;
            _userRepository = userRepository;

            RuleFor(x => x).MustAsync(UserLockRegistrationExists).WithMessage("UserLock Not Found");
            RuleFor(x => x.LockId).MustAsync(ActiveLockExists).WithMessage("Lock Not Found");
            RuleFor(x => x.UserId).MustAsync(ActiveUserExists).WithMessage("User Not Found");
        }


        private async Task<bool> UserLockRegistrationExists(UnlockCommand command, CancellationToken ct)
        {
            var userLock = await _lockRepository.GetUserLockAsync(command.UserId, command.LockId);
            return userLock != null;
        }

        private async Task<bool> ActiveLockExists(Guid lockId, CancellationToken ct)
        {
            var userLock = await _lockRepository.GetLockAsync(lockId);
            return userLock != null;
        }

        private async Task<bool> ActiveUserExists(Guid userId, CancellationToken ct)
        {
            var userLock = await _userRepository.GetUserAsync(userId);
            return userLock != null;
        }

    }
}