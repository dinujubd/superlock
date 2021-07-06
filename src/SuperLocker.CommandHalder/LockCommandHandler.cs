using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SuperLocker.Core;
using SuperLocker.Core.Command;
using SuperLocker.Core.Repositories;

namespace SuperLocker.CommandHandler
{
    public class LockCommandHandler : ICommandHandler<LockCommand>
    {

        private readonly ILockRepository _lockRepository;
        public LockCommandHandler(ILockRepository lockRepository)
        {
            _lockRepository = lockRepository;
        }
        public async Task Consume(ConsumeContext<LockCommand> context)
        {
            await _lockRepository.Lock(context.Message);
        }
    }
}
