using System;
using System.Data;
using Mono.Data.Sqlite;

namespace StoreBillingSystem.Database
{
    public class DatabaseManager
    {
        private static IDbConnection _connection;
        private static readonly object _lockObject = new object();

        // Private constructor to prevent creating instances outside the class.
        private DatabaseManager()
        {
        }

        // Public method to get the shared database connection instance.
        public static IDbConnection GetConnection()
        {
            // Specify the relative path to your SQLite database file.
            string dbFileName = "billing.db";

            string dbFolder = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Databases");
            string dbFilePath = System.IO.Path.Combine(dbFolder, dbFileName);

            if (!System.IO.Directory.Exists(dbFolder))
            {
                System.IO.Directory.CreateDirectory(dbFolder);
            }
            // Create the database file if it doesn't exist
            if (!System.IO.File.Exists(dbFilePath))
            {
                SqliteConnection.CreateFile(dbFilePath);
            }

            lock (_lockObject)
            {
                if (_connection == null)
                {
                    // Create and open the SQLite connection.
                    _connection = new SqliteConnection($"Data Source={dbFilePath};Version=3;"); 
                    _connection.Open();
                }

                return _connection;
            }
        }

        // Public method to close the shared database connection.
        public static void CloseConnection()
        {
            lock (_lockObject)
            {
                if (_connection != null)
                {
                    _connection.Close();
                    _connection = null;
                }
            }
        }
    }
}
