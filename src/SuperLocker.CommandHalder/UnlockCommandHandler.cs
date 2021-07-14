using System;
using System.Threading.Tasks;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SuperLocker.Application;
using SuperLocker.Application.Commands;
using SuperLocker.Domain.Entities.Aggregates.Lock.Repository;

namespace SuperLocker.CommandHandler
{
    public class UnlockCommandHandler : ICommandHandler<UnlockCommand>
    {
        private readonly ILockRepository _lockRepository;
        private readonly ILogger<UnlockCommand> _logger;
        private readonly IValidator<UnlockCommand> _validator;

        public UnlockCommandHandler(ILockRepository lockRepository, IValidator<UnlockCommand> validator,
            ILogger<UnlockCommand> logger)
        {
            _validator = validator;
            _lockRepository = lockRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UnlockCommand> context)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(context.Message);
                if (!validationResult.IsValid)
                {
                    validationResult.Errors.ForEach(x =>
                    {
                        _logger.LogError("UNLOCKED.VALIDATION.ERROR {0} ->", x.ErrorMessage);
                    });
                }
                else
                {
                    await _lockRepository.Unlock(context.Message.UserId, context.Message.LockId);

                    _logger.LogInformation("UNLOCKED.SUCCESS lockId: {0}, userId: {1} ->", context.Message.LockId,
                        context.Message.UserId);
                }
            }
            catch (MySqlException dbExcpetion)
            {
                _logger.LogError(dbExcpetion, "DATABASE.FAILED.EXCEPTION");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UNLOCKED.FAILED.EXCEPTION");
                throw;
            }
        }
    }
}