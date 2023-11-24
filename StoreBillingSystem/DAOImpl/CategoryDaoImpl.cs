using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;

using StoreBillingSystem.DAO;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Util;

namespace StoreBillingSystem.DAOImpl
{
    public class CategoryDaoImpl : ICategoryDao
    {
        private SqliteConnection _conn;
        private string _tableName;

        public CategoryDaoImpl(SqliteConnection conn)
        {
            _conn = conn;
            _tableName = StoreDbTable.Category.ToString();
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

        public bool Insert(Category category)
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {

                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Insert a single record
                    command.CommandText = $"INSERT INTO {_tableName} (NAME) VALUES (@Name); SELECT last_insert_rowid()";
                    command.Parameters.AddWithValue("@Name", category.Name);
                    //command.ExecuteNonQuery();


                    long generatedId = (long)command.ExecuteScalar();
                    Console.WriteLine($"Auto-generated ID: {generatedId}");

                    category.Id = (int)generatedId;

                    transaction.Commit();
                    return true;
                }
            }
        }

        public bool IsRecordExists(string name)
        {
            string query = $"SELECT * FROM {_tableName} WHERE NAME=@Name";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                // Add parameters to the query
                command.Parameters.AddWithValue("@Name", name);

                // Execute the query and check if any rows are returned
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public Category Read(int id)
        {
            Category category = null;
            using (SqliteCommand command = _conn.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM {_tableName} WHERE ID = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        category = new Category()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                    }
                }
            }
            return category;
        }

        public IList<Category> ReadAll()
        {
            IList<Category> categories = new List<Category>();

            using (SqliteCommand command = _conn.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM {_tableName}";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category() { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                    }
                }
            }
            return categories;
        }

        public bool Update(Category category)
        {
            // Create the SQL command to update the record
            string updateQuery = $"UPDATE {_tableName} SET NAME = @NewName WHERE ID = @Id";

            using (SqliteCommand command = new SqliteCommand(updateQuery, _conn))
            {
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Add the parameters to the command
                    command.Parameters.AddWithValue("@NewName", category.Name);
                    command.Parameters.AddWithValue("@Id", category.Id);

                    // Execute the UPDATE command
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
