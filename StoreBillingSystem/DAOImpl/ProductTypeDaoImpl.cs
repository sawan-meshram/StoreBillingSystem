using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;

using StoreBillingSystem.DAO;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Util;
namespace StoreBillingSystem.DAOImpl
{
    public class ProductTypeDaoImpl : IProductTypeDao
    {
        private SqliteConnection _conn;
        private string _tableName;

        public ProductTypeDaoImpl(SqliteConnection conn)
        {
            _conn = conn;
            _tableName = StoreDbTable.ProductType.ToString();
        }

        public bool Delete(int id)
        {
            // Create the SQL command to delete the record
            string deleteQuery = $"DELETE FROM {_tableName} WHERE ID = @Id";

            using (SqliteCommand command = new SqliteCommand(deleteQuery, _conn))
            {
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Add the ID parameter to the command
                    command.Parameters.AddWithValue("@Id", id);

                    // Execute the DELETE command
                    int rowsAffected = command.ExecuteNonQuery();

                    transaction.Commit();

                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Insert(ProductType productType)
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {

                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Insert a single record
                    command.CommandText = $"INSERT INTO {_tableName} (NAME, ABBR) VALUES (@Name, @Abbr); SELECT last_insert_rowid();";

                    command.Parameters.AddWithValue("@Name", productType.Name);
                    command.Parameters.AddWithValue("@Abbr", productType.Abbr);


                    long generatedId = (long)command.ExecuteScalar();
                    Console.WriteLine($"Auto-generated ID: {generatedId}");

                    productType.Id = (int)generatedId;

                    transaction.Commit();
                    return true;
                }
            }
        }

        public bool IsRecordExists(string name, string abbr)
        {
            string query = $"SELECT * FROM {_tableName} WHERE NAME=@Name AND ABBR=@Abbr";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                // Add parameters to the query
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Abbr", abbr);


                // Execute the query and check if any rows are returned
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public ProductType Read(int id)
        {

            ProductType productType = null;
            using (SqliteCommand command = _conn.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM {_tableName} WHERE ID = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productType = new ProductType()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            Name = reader.GetString(reader.GetOrdinal("NAME")),
                            Abbr = reader.GetString(reader.GetOrdinal("ABBR"))
                        };
                    }
                }
            }
            return productType;
        }

        public IList<ProductType> ReadAll()
        {
            IList<ProductType> productTypes = new List<ProductType>();

            using (SqliteCommand command = _conn.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM {_tableName}";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productTypes.Add(new ProductType() 
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            Name = reader.GetString(reader.GetOrdinal("NAME")),
                            Abbr = reader.GetString(reader.GetOrdinal("ABBR")) 
                        });
                    }
                }
            }
            return productTypes;
        }

        public bool Update(ProductType productType)
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {

                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Insert a single record
                    command.CommandText = $"UPDATE {_tableName} SET Name = @Name, Abbr = @Abbr WHERE ID = @Id";
                    command.Parameters.AddWithValue("@Name", productType.Name);
                    command.Parameters.AddWithValue("@Abbr", productType.Abbr);
                    command.Parameters.AddWithValue("@Id", productType.Id);

                    int rowsAffected = command.ExecuteNonQuery();

                    transaction.Commit();

                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
