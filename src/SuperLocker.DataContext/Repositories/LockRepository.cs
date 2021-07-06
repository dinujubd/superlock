using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SuperLocker.Core.Command;
using SuperLocker.Core.Repositories;

namespace SuperLocker.DataContext.Repositories {
    public class LockRepository : ILockRepository
    {
        private readonly ILogger<LockCommand> _logger;

        public LockRepository(ILogger<LockCommand> logger)
        {
            _logger = logger;
        }
        public Task Lock(LockCommand lockInfo)
        {
            _logger.LogInformation("coming to papa");
            return Task.CompletedTask;
        }
    }
}