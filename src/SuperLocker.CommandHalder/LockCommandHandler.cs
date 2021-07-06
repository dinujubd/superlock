using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SuperLocker.Core;
using SuperLocker.Core.Command;

namespace SuperLocker.CommandHandler
{
    public class LockCommandHandler : ICommandHandler<LockCommand>
    {
        private readonly ILogger<LockCommand> _logger;
        public LockCommandHandler(ILogger<LockCommand> logger)
        {
            _logger = logger;   
        }
        public Task Consume(ConsumeContext<LockCommand> context)
        {
            _logger.LogInformation("Received Text: {Text}", context.Message.LockId.ToString());
            return Task.CompletedTask;
        }
    }
}
