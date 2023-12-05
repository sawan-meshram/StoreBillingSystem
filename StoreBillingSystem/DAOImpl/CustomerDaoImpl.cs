using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;


using StoreBillingSystem.DAO;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Util;

namespace StoreBillingSystem.DAOImpl
{
    public class CustomerDaoImpl : ICustomerDao
    {
        private SqliteConnection _conn;
        private string _tableName;

        public CustomerDaoImpl(SqliteConnection conn)
        {
            _conn = conn;
            _tableName = StoreDbTable.Customer.ToString();
        }

        public ISet<string> CustomerNames()
        {
            ISet<string> customerNames = new HashSet<string>();

            string query = $"SELECT NAME FROM {_tableName}";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if(!customerNames.Contains(reader.GetString(reader.GetOrdinal("NAME"))))
                            customerNames.Add(reader.GetString(reader.GetOrdinal("NAME")));
                    }
                }
            }
            return customerNames;
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

        public int GetNewCustomerId()
        {
            string query = $"SELECT MAX(ID) FROM {_tableName}";
            int newCustomerId = 0;
            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    // If there are existing ProductIds, increment the maximum one
                    newCustomerId = Convert.ToInt32(result) + 1;
                }
                else
                {
                    newCustomerId = 1;
                }
            }
            return newCustomerId;
        }

        public bool Insert(Customer customer)
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {

                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Insert a single record
                    command.CommandText = $"INSERT INTO {_tableName} (ID, NAME, ADDRESS, PHONE, REGISTER_DATE) VALUES (@Id, @Name, @Address, @Phone, @RegisterDate); SELECT last_insert_rowid()";
                    command.Parameters.AddWithValue("@Id", customer.Id);
                    command.Parameters.AddWithValue("@Name", customer.Name);
                    command.Parameters.AddWithValue("@Address", customer.Address);
                    command.Parameters.AddWithValue("@Phone", customer.PhoneNumber);
                    command.Parameters.AddWithValue("@RegisterDate", customer.RegisterDate);

                    long generatedId = (long)command.ExecuteScalar();
                    Console.WriteLine($"Auto-generated ID: {generatedId}");

                    customer.Id = (int)generatedId;

                    transaction.Commit();
                    return true;
                }
            }
        }

        public bool IsRecordExists(string name, long phoneNumber)
        {
            string query = $"SELECT * FROM {_tableName} WHERE NAME=@Name AND PHONE=@Phone";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                // Add parameters to the query
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Phone", phoneNumber);

                // Execute the query and check if any rows are returned
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public bool IsRecordExists(long phoneNumber)
        {
            string query = $"SELECT * FROM {_tableName} WHERE PHONE=@Phone";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                // Add parameters to the query
                command.Parameters.AddWithValue("@Phone", phoneNumber);

                // Execute the query and check if any rows are returned
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public IList<string> Phones()
        {
            IList<string> phones = new List<string>();

            string query = $"SELECT PHONE FROM {_tableName}";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        phones.Add(reader.GetInt64(reader.GetOrdinal("PHONE")).ToString());
                    }
                }
            }
            return phones;
        }

        public Customer Read(int id)
        {
            string query = $"SELECT * FROM {_tableName} WHERE ID = @Id";

            Customer customer = null;

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customer = new Customer()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            Name = reader.GetString(reader.GetOrdinal("NAME")),
                            Address = reader.GetString(reader.GetOrdinal("ADDRESS")),
                            PhoneNumber = reader.GetInt64(reader.GetOrdinal("PHONE")),
                            RegisterDate = reader.GetString(reader.GetOrdinal("REGISTER_DATE"))
                        };
                    }
                }
            }

            return customer;
        }

        public IList<Customer> Read(string customerName)
        {
            IList<Customer> customers = new List<Customer>();

            string query = $"SELECT * FROM {_tableName} WHERE NAME = @CustomerName";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@CustomerName", customerName);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            Name = reader.GetString(reader.GetOrdinal("NAME")),
                            Address = reader.GetString(reader.GetOrdinal("ADDRESS")),
                            PhoneNumber = reader.GetInt64(reader.GetOrdinal("PHONE")),
                            RegisterDate = reader.GetString(reader.GetOrdinal("REGISTER_DATE"))
                        });
                    }
                }
            }
            return customers;
        }

        public IList<Customer> ReadAll()
        {
            IList<Customer> customers = new List<Customer>();

            string query = $"SELECT * FROM {_tableName}";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            Name = reader.GetString(reader.GetOrdinal("NAME")),
                            Address = reader.GetString(reader.GetOrdinal("ADDRESS")),
                            PhoneNumber = reader.GetInt64(reader.GetOrdinal("PHONE")),
                            RegisterDate = reader.GetString(reader.GetOrdinal("REGISTER_DATE"))
                        }); 
                    }
                }
            }
            return customers;
        }

        public bool Update(Customer customer)
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {
                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Insert a single record
                    command.CommandText = $"UPDATE {_tableName} SET NAME=@Name, ADDRESS=@Address, PHONE=@Phone, REGISTER_DATE=@RegisterDate WHERE ID = @Id";
                    command.Parameters.AddWithValue("@Name", customer.Name);
                    command.Parameters.AddWithValue("@Address", customer.Address);
                    command.Parameters.AddWithValue("@Phone", customer.PhoneNumber);
                    command.Parameters.AddWithValue("@RegisterDate", customer.RegisterDate);
                    command.Parameters.AddWithValue("@Id", customer.Id);

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
