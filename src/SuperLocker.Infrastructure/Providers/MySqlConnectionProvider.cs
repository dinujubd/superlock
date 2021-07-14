using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using SuperLocker.Shared;

namespace SuperLocker.Infrastructure.Providers
{
    public class MySqlConnectionProvider : IDatabaseConnectionProvider<MySqlConnection>
    {
        private readonly IOptions<DatabaseConfigurations> _config;

        public MySqlConnectionProvider(IOptions<DatabaseConfigurations> config)
        {
            _config = config;
        }

        public MySqlConnection Get()
        {
            return new(_config.Value.ConnectionString);
        }
    }
}