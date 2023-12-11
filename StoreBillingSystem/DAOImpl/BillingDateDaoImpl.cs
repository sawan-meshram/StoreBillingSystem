using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;

using StoreBillingSystem.DAO;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Util;

namespace StoreBillingSystem.DAOImpl
{
    public class BillingDateDaoImpl : IBillingDateDao
    {
        private SqliteConnection _conn;
        private readonly string _tableName;
        public BillingDateDaoImpl(SqliteConnection conn)
        {
            _conn = conn;
            _tableName = StoreDbTable.BillingDate.ToString();
        }

        public bool Delete(long id)
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

        public bool Insert(BillingDate billingDate)
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {
                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Insert a single record
                    command.CommandText = $"INSERT INTO {_tableName} (BILLING_DATE) VALUES (@BillDate); SELECT last_insert_rowid()";
                    command.Parameters.AddWithValue("@BillDate", billingDate.BillDate);

                    long generatedId = (long)command.ExecuteScalar();
                    Console.WriteLine($"Auto-generated ID: {generatedId}");

                    billingDate.Id = generatedId;

                    transaction.Commit();
                    return true;
                }
            }
        }

        public bool IsRecordExists(string billingDate)
        {
            string query = $"SELECT * FROM {_tableName} WHERE BILLING_DATE=@BillDate";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                // Add parameters to the query
                command.Parameters.AddWithValue("@BillDate", billingDate);

                // Execute the query and check if any rows are returned
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public BillingDate Read(long id)
        {
            string query = $"SELECT * FROM {_tableName} WHERE ID = @Id";

            BillingDate billingDate = null;

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        billingDate = new BillingDate
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            BillDate = reader.GetString(reader.GetOrdinal("BILLING_DATE"))
                        };
                    }
                }
            }
            return billingDate;
        }

        public BillingDate Read(string billingDate)
        {
            string query = $"SELECT * FROM {_tableName} WHERE BILLING_DATE=@BillDate";

            BillingDate billDate = null;

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@BillDate", billingDate);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        billDate = new BillingDate
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            BillDate = reader.GetString(reader.GetOrdinal("BILLING_DATE"))
                        };
                    }
                }
            }
            return billDate;
        }

        public IList<BillingDate> ReadAll()
        {
            IList<BillingDate> billingDates = new List<BillingDate>();

            string query = $"SELECT * FROM {_tableName}";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        billingDates.Add(new BillingDate
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            BillDate = reader.GetString(reader.GetOrdinal("BILLING_DATE"))
                        });
                    }
                }
            }
            return billingDates;
        }
    }
}
