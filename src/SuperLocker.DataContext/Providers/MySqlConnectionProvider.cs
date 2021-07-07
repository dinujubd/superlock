using MySql.Data.MySqlClient;

namespace SuperLocker.DataContext.Providers
{
    public class MySqlConnectionProvider : IDatabaseConnectionProvider<MySqlConnection>
    {
        private string _connectionString = "Server=localhost;Database=AppDBSuperLock;Uid=root;Pwd=rpass;";
        public MySqlConnection Get()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}   