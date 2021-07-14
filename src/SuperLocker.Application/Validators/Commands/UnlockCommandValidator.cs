using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using SuperLocker.Application.Commands;
using SuperLocker.Domain.Entities.Aggregates.Lock.Repository;
using SuperLocker.Domain.Entities.Aggregates.User.Repository;

namespace SuperLocker.Application.Validators.Commands
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