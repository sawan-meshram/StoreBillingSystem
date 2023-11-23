using System;
using Mono.Data.Sqlite;
namespace StoreBillingSystem.Database
{
    public class StoreDbTableManager
    {

        private static StoreDbTableManager instance;

        private static SqliteConnection _conn = null;
        private StoreDbTableManager()
        {
            _conn = DatabaseManager.GetConnection();

            //Initialise all tables
            InitTables();
        }

        public static StoreDbTableManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StoreDbTableManager();
                }
                return instance;
            }
        }

        private void InitTables()
        {
            CreateCategoryTable();
            CreateProductTypeTable();
        }

        private void CreateCategoryTable()
        {
            // Create a new SQLite command.
            using (SqliteCommand command = _conn.CreateCommand())
            {
                // Specify the SQL command to create a table.
                command.CommandText = @"CREATE TABLE IF NOT EXISTS Category (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT, 
                    NAME TEXT NOT NULL
                    );";

                // Execute the SQL command.
                int row = command.ExecuteNonQuery();
                if (row == 1) Console.WriteLine("'Category' Table Created...");
            }
        }

        private void CreateProductTypeTable()
        {
            // Create a new SQLite command.
            using (SqliteCommand command = _conn.CreateCommand())
            {
                // Specify the SQL command to create a table.
                command.CommandText = @"CREATE TABLE IF NOT EXISTS ProductType (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT, 
                    NAME TEXT NOT NULL,
                    ABBR TEXT NOT NULL
                    );";

                // Execute the SQL command.
                int row = command.ExecuteNonQuery();
                if (row == 1) Console.WriteLine("'ProductType' Table Created...");
            }
        }




    }
}
