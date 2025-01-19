using Microsoft.Data.Sqlite;

namespace CryptoCheck.Infra
{
    public class DatabaseConnection
    {
        private string _connectionString;
        public DatabaseConnection(string connectionString)
        {
            this._connectionString = connectionString;
        }
        public Object PerformQuery(Func<SqliteCommand, Object> action)
        {
            using (var connection = new SqliteConnection(this._connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                return action.Invoke(command);
            }
        }
    }
}
