using System;
using System.Collections.Generic;
using System.IO;
using MySql.Data.MySqlClient;
using ThrowawayDb.MySql;
using Xunit;

namespace SuperLocker.Intgration.Tests
{
    public class DatabaseIntegrationTests
    {
        [Fact]
        public void CanConnectAndSelectFromDatabase()
        {
            using var database = ThrowawayDatabase.Create("Server=localhost;Uid=root;Pwd=rpass;");
            using var connection = new MySqlConnection(database.ConnectionString);
            connection.Open();

            using var reader = connection.ExecuteReader("SELECT 1");
            var result = Convert.ToInt32(reader.Read());

            Assert.Equal(1, result);
        }

        [Fact]
        public void CanApplyExpectedSchema()
        {
            var expectedTables = new List<string>
            {
                "Roles", "Users", "UserUnlockActivity", "UserRoles", "UserLocks", "Locks"
            };

            using var database = ThrowawayDatabase.Create("Server=localhost;Uid=root;Pwd=rpass;");
            using var connection = new MySqlConnection(database.ConnectionString);

            connection.Open();

            var sqlQuery = File.ReadAllText("./sql_command.txt");

            connection.ExecuteNonQuery(sqlQuery);

            var reader = connection.ExecuteReader(
                $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA='{database.Name}'");

            var tables = new List<string>();


            while (reader.Read()) tables.Add(reader[0].ToString());

            expectedTables.Sort();
            tables.Sort();

            Assert.Equal(expectedTables, tables);
        }
    }
}