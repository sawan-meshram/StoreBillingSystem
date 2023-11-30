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
            CreateProductTable();
            CreateProductPurchaseTable();
            CreateProductSellingTable();
            CreateCustomerTable();
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

        private void CreateProductTable()
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {
                // Specify the SQL command to create a table.
                command.CommandText = @"CREATE TABLE IF NOT EXISTS Product (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT, 
                    NAME TEXT NOT NULL UNIQUE,
                    QTY REAL DEFAULT 0,
                    Category_ID INTEGER,
                    ProductType_ID INTEGER,
                    FOREIGN KEY (Category_ID) REFERENCES Category (ID) 
                        ON DELETE CASCADE 
                        ON UPDATE NO ACTION,
                    FOREIGN KEY (ProductType_ID) REFERENCES ProductType (ID) 
                        ON DELETE CASCADE 
                        ON UPDATE NO ACTION
                    );";

                // Execute the SQL command.
                int row = command.ExecuteNonQuery();
                if (row == 1) Console.WriteLine("'Product' Table Created...");
            }
        }

        private void CreateProductPurchaseTable()
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {
                // Specify the SQL command to create a table.
                command.CommandText = @"CREATE TABLE IF NOT EXISTS ProductPurchase (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT, 
                    PURCHASE_DATE TEXT NULL,
                    QTY REAL DEFAULT 0,
                    PRICE REAL DEFAULT 0,
                    CGST_PERCENT REAL DEFAULT 0,
                    SGST_PERCENT REAL DEFAULT 0,
                    MFG_DATE TEXT NULL,
                    EXP_DATE TEXT NULL,
                    BATCH TEXT NULL,
                    Product_ID INTEGER,
                    FOREIGN KEY (Product_ID) REFERENCES Product (ID) 
                        ON DELETE CASCADE 
                        ON UPDATE NO ACTION
                    );";

                // Execute the SQL command.
                int row = command.ExecuteNonQuery();
                if (row == 1) Console.WriteLine("'ProductPurchase' Table Created...");
            }
        }

        private void CreateProductSellingTable()
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {
                // Specify the SQL command to create a table.
                command.CommandText = @"CREATE TABLE IF NOT EXISTS ProductSelling (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT, 
                    PRICE_A REAL DEFAULT 0,
                    DISCOUNT_PRICE_A REAL DEFAULT 0,
                    PRICE_B REAL DEFAULT 0,
                    DISCOUNT_PRICE_B REAL DEFAULT 0,
                    PRICE_C REAL DEFAULT 0,
                    DISCOUNT_PRICE_C REAL DEFAULT 0,
                    PRICE_D REAL DEFAULT 0,
                    DISCOUNT_PRICE_D REAL DEFAULT 0,
                    CGST_PERCENT REAL DEFAULT 0,
                    SGST_PERCENT REAL DEFAULT 0,
                    Product_ID INTEGER,
                    FOREIGN KEY (Product_ID) REFERENCES Product (ID) 
                        ON DELETE CASCADE 
                        ON UPDATE NO ACTION
                    );";

                // Execute the SQL command.
                int row = command.ExecuteNonQuery();
                if (row == 1) Console.WriteLine("'ProductSelling' Table Created...");
            }
        }


        private void CreateCustomerTable()
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {
                // Specify the SQL command to create a table.
                command.CommandText = @"CREATE TABLE IF NOT EXISTS Customer (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT, 
                    NAME TEXT NOT NULL,
                    ADDRESS TEXT NOT NULL,
                    PHONE INTEGER UNIQUE,
                    REGISTER_DATE TEXT NULL);";

                // Execute the SQL command.
                int row = command.ExecuteNonQuery();
                if (row == 1) Console.WriteLine("'Customer' Table Created...");
            }
        }
    }
}
