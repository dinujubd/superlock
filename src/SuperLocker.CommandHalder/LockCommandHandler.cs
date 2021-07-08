using FluentValidation;
using MassTransit;
using SuperLocker.Core;
using SuperLocker.Core.Command;
using SuperLocker.Core.Repositories;
using System.Threading.Tasks;

namespace SuperLocker.CommandHandler
{
    public class UnlockCommandHandler : ICommandHandler<UnlockCommand>
    {
        private readonly ILockRepository _lockRepository;
        private readonly IValidator<UnlockCommand> _valdator;
        public UnlockCommandHandler(ILockRepository lockRepository, IValidator<UnlockCommand> valdator)
        {
            _valdator = valdator;
            _lockRepository = lockRepository;
        }
        public async Task Consume(ConsumeContext<UnlockCommand> context)
        {
            var validationResult = await _valdator.ValidateAsync(context.Message);
            
            if (validationResult.IsValid)
            {
                await _lockRepository.Unlock(context.Message);
            }
        }
    }
}
