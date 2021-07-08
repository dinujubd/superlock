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
