using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Logging;
using SuperLocker.Core;
using SuperLocker.Core.Command;
using SuperLocker.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace SuperLocker.CommandHandler
{
    public class UnlockCommandHandler : ICommandHandler<UnlockCommand>
    {
        private readonly ILockRepository _lockRepository;
        private readonly ILogger<UnlockCommand> _logger;
        private readonly IValidator<UnlockCommand> _valdator;
        public UnlockCommandHandler(ILockRepository lockRepository, IValidator<UnlockCommand> valdator, ILogger<UnlockCommand> logger)
        {
            _valdator = valdator;
            _lockRepository = lockRepository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<UnlockCommand> context)
        {
            try
            {
                var validationResult = await _valdator.ValidateAsync(context.Message);
                if (!validationResult.IsValid)
                {
                    validationResult.Errors.ForEach(x =>
                    {
                        _logger.LogError("UNLOCKED.VALIDATION.ERROR {0} ->", x.ErrorMessage);
                    });
                }
                else
                {
                    await _lockRepository.Unlock(context.Message);

                    _logger.LogInformation("UNLOCKED.SUCCESS lockId: {0}, userId: {1} ->", context.Message.LockId, context.Message.UserId);
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e,"UNLOCKED.FAILED.EXCEPTION");
                throw;
            }

        }
    }
}
