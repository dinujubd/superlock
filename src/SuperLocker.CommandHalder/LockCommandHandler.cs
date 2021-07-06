using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SuperLocker.Core;
using SuperLocker.Core.Command;
using SuperLocker.Core.Repositories;

namespace SuperLocker.CommandHandler
{
    public class UnlockCommandHandler : ICommandHandler<UnlockCommand>
    {

        private readonly ILockRepository _lockRepository;
        public UnlockCommandHandler(ILockRepository lockRepository)
        {
            _lockRepository = lockRepository;
        }
        public async Task Consume(ConsumeContext<UnlockCommand> context)
        {
            await _lockRepository.Unlock(context.Message);
        }
    }
}
