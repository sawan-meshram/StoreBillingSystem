using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;

using StoreBillingSystem.DAO;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Util;


namespace StoreBillingSystem.DAOImpl
{
    public class BillingDaoImpl : IBillingDao
    {
        private SqliteConnection _conn;
        private string _tableName;

        private ICustomerDao _customerDao;
        private IBillingDateDao _billingDateDao;

        public BillingDaoImpl(SqliteConnection conn)
        {
            _conn = conn;
            _tableName = StoreDbTable.Billing.ToString();

            _customerDao = new CustomerDaoImpl(conn);
            _billingDateDao = new BillingDateDaoImpl(conn);
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

        public long GetNewBillingNumber(BillingDate billingDate)
        {
            string query = $"SELECT COUNT(ID) FROM {_tableName} WHERE BillingDate_ID = @BillingDateId";
            long newBillingNumber = 0;
            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@BillingDateId", billingDate.Id);

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    // If there are existing ProductIds, increment the maximum one
                    newBillingNumber = Convert.ToInt64(result) + 1;
                }
                else
                {
                    newBillingNumber = 1;
                }
            }
            return newBillingNumber;
        }

        public bool Insert(Billing billing)
        {

            using (SqliteCommand command = _conn.CreateCommand())
            {

                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Insert a single record
                    command.CommandText = $"INSERT INTO {_tableName} (BILLING_NUMBER, BILLING_DATE, Customer_ID, BillingDate_ID, GROSS_AMOUNT, GST_PRICE, DISCOUNT_PRICE, NET_AMOUNT) " +
                    	$"VALUES (@BillNumber, @BillDate, @CustomerId, @BillDateId, @GrossAmt, @GstPrice, @DiscountPrice, @NetAmt); SELECT last_insert_rowid()";
                    command.Parameters.AddWithValue("@BillNumber", billing.BillingNumber);
                    command.Parameters.AddWithValue("@BillDate", billing.BillingDateTime);
                    command.Parameters.AddWithValue("@CustomerId", billing.Customer.Id);
                    command.Parameters.AddWithValue("@BillDateId", billing.BillingDate.Id);
                    command.Parameters.AddWithValue("@GrossAmt", billing.GrossAmount);
                    command.Parameters.AddWithValue("@GstPrice", billing.GSTPrice);
                    command.Parameters.AddWithValue("@DiscountPrice", billing.DiscountPrice);
                    command.Parameters.AddWithValue("@NetAmt", billing.NetAmount);
                    //command.ExecuteNonQuery();


                    long generatedId = (long)command.ExecuteScalar();
                    Console.WriteLine($"Auto-generated ID: {generatedId}");

                    billing.Id = generatedId;

                    transaction.Commit();
                    return true;
                }
            }
        }

        public Billing Read(long id)
        {
            string query = $"SELECT * FROM {_tableName} WHERE ID = @Id";

            Billing billing = null;

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        billing = new Billing
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            BillingNumber = reader.GetInt64(reader.GetOrdinal("BILLING_NUMBER")),
                            BillingDateTime = reader.GetString(reader.GetOrdinal("BILLING_DATE")),
                            GrossAmount = reader.GetDouble(reader.GetOrdinal("GROSS_AMOUNT")),
                            GSTPrice = reader.GetDouble(reader.GetOrdinal("GST_PRICE")),
                            DiscountPrice = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE")),
                            NetAmount = reader.GetDouble(reader.GetOrdinal("NET_AMOUNT")),
                        };
                        billing.Customer = _customerDao.Read(reader.GetInt32(reader.GetOrdinal("Customer_ID")));
                        billing.BillingDate = _billingDateDao.Read(reader.GetInt64(reader.GetOrdinal("BillingDate_ID")));
                    }
                }
            }

            return billing;
        }

        public Billing Read(long billingNumber, BillingDate billingDate)
        {
            string query = $"SELECT * FROM {_tableName} WHERE BILLING_NUMBER = @BillingNumber AND BillingDate_ID = @BillingDateId";

            Billing billing = null;

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@BillingNumber", billingNumber);
                command.Parameters.AddWithValue("@BillingDateId", billingDate.Id);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        billing = new Billing
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            BillingNumber = reader.GetInt64(reader.GetOrdinal("BILLING_NUMBER")),
                            BillingDateTime = reader.GetString(reader.GetOrdinal("BILLING_DATE")),
                            GrossAmount = reader.GetDouble(reader.GetOrdinal("GROSS_AMOUNT")),
                            GSTPrice = reader.GetDouble(reader.GetOrdinal("GST_PRICE")),
                            DiscountPrice = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE")),
                            NetAmount = reader.GetDouble(reader.GetOrdinal("NET_AMOUNT")),
                        };
                        billing.Customer = _customerDao.Read(reader.GetInt32(reader.GetOrdinal("Customer_ID")));
                        billing.BillingDate = _billingDateDao.Read(reader.GetInt64(reader.GetOrdinal("BillingDate_ID")));
                    }
                }
            }

            return billing;
        }

        public IList<Billing> ReadAll()
        {
            IList<Billing> billings = new List<Billing>();

            string query = $"SELECT * FROM {_tableName}";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Billing billing = new Billing
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            BillingNumber = reader.GetInt64(reader.GetOrdinal("BILLING_NUMBER")),
                            BillingDateTime = reader.GetString(reader.GetOrdinal("BILLING_DATE")),
                            GrossAmount = reader.GetDouble(reader.GetOrdinal("GROSS_AMOUNT")),
                            GSTPrice = reader.GetDouble(reader.GetOrdinal("GST_PRICE")),
                            DiscountPrice = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE")),
                            NetAmount = reader.GetDouble(reader.GetOrdinal("NET_AMOUNT")),
                        };
                        billing.Customer = _customerDao.Read(reader.GetInt32(reader.GetOrdinal("Customer_ID")));
                        billing.BillingDate = _billingDateDao.Read(reader.GetInt64(reader.GetOrdinal("BillingDate_ID")));

                        billings.Add(billing);
                    }
                }
            }

            return billings;
        }
    }
}
