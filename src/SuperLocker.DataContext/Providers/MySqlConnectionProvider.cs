using MySql.Data.MySqlClient;

namespace SuperLocker.DataContext.Providers
{
    public class MySqlConnectionProvider : IDatabaseConnectionProvider<MySqlConnection>
    {
        private readonly string _connectionString = "Server=localhost;Database=AppDBSuperLock;Uid=root;Pwd=rpass;";

        public MySqlConnection Get()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}