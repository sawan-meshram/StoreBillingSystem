using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;

using StoreBillingSystem.DAO;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Util;

namespace StoreBillingSystem.DAOImpl
{
    public class PaymentDaoImpl : IPaymentDao
    {
        private SqliteConnection _conn;
        private string _tableName;

        private IBillingDao _billingDao;

        public PaymentDaoImpl(SqliteConnection conn)
        {
            _conn = conn;
            _tableName = StoreDbTable.Payment.ToString();

            _billingDao = new BillingDaoImpl(conn);
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

        public bool Insert(Payment payment)
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {

                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Insert a single record
                    command.CommandText = $"INSERT INTO {_tableName} (Billing_ID, PAYMENT_MODE, STATUS, PAID_AMOUNT, PAID_DATE, BALANCE_AMOUNT, BALANCE_PAID_DATE) " +
                        $"VALUES (@BillingId, @PaymentMode, @Status, @PaidAmt, @PaidDate, @BalanceAmt, @BalancePaidDate); SELECT last_insert_rowid()";
                    command.Parameters.AddWithValue("@BillingId", payment.Billing.Id);
                    command.Parameters.AddWithValue("@PaymentMode", payment.PaymentMode.ToString());
                    command.Parameters.AddWithValue("@Status", payment.Status.ToString());
                    command.Parameters.AddWithValue("@PaidAmt", payment.PaidAmount);
                    command.Parameters.AddWithValue("@PaidDate", payment.PaidDate);
                    command.Parameters.AddWithValue("@BalanceAmt", payment.BalanceAmount);
                    command.Parameters.AddWithValue("@BalancePaidDate", payment.BalancePaidDate);


                    long generatedId = (long)command.ExecuteScalar();
                    Console.WriteLine($"Auto-generated ID: {generatedId}");

                    payment.Id = generatedId;

                    transaction.Commit();
                    return true;
                }
            }
        }

        public bool IsRecordExists(long billingId)
        {
            string query = $"SELECT * FROM {_tableName} WHERE Billing_ID=@BillingId";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                // Add parameters to the query
                command.Parameters.AddWithValue("@BillingId", billingId);

                // Execute the query and check if any rows are returned
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public IList<Payment> ReadAll()
        {
            string query = $"SELECT * FROM {_tableName}";

            IList<Payment> payments = new List<Payment>();

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Payment payment = new Payment
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            PaidAmount = reader.GetDouble(reader.GetOrdinal("PAID_AMOUNT")),
                            PaidDate = reader.GetString(reader.GetOrdinal("PAID_DATE")),
                            BalanceAmount = reader.GetDouble(reader.GetOrdinal("BALANCE_AMOUNT")),
                            BalancePaidDate = reader.IsDBNull(reader.GetOrdinal("BALANCE_PAID_DATE")) ? "" : reader.GetString(reader.GetOrdinal("BALANCE_PAID_DATE"))
                        };
                        payment.Billing = _billingDao.Read(reader.GetInt64(reader.GetOrdinal("Billing_ID")));
                        payment.PaymentMode = (PaymentMode)Enum.Parse(typeof(PaymentMode), reader.GetString(reader.GetOrdinal("PAYMENT_MODE")), true);
                        payment.Status = (BillingStatus)Enum.Parse(typeof(BillingStatus), reader.GetString(reader.GetOrdinal("STATUS")), true);

                        payments.Add(payment);
                    }
                }
            }

            return payments;
        }

        public Payment ReadByBillingId(long billingId)
        {
            string query = $"SELECT * FROM {_tableName} WHERE Billing_ID = @BillingId";

            Payment payment = null;

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@BillingId", billingId);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        payment = new Payment
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            PaidAmount = reader.GetDouble(reader.GetOrdinal("PAID_AMOUNT")),
                            PaidDate = reader.GetString(reader.GetOrdinal("PAID_DATE")),
                            BalanceAmount = reader.GetDouble(reader.GetOrdinal("BALANCE_AMOUNT")),
                            BalancePaidDate = reader.GetString(reader.GetOrdinal("BALANCE_PAID_DATE"))
                        };
                        payment.Billing = _billingDao.Read(reader.GetInt64(reader.GetOrdinal("Billing_ID")));
                        payment.PaymentMode = (PaymentMode)Enum.Parse(typeof(PaymentMode), reader.GetString(reader.GetOrdinal("PAYMENT_MODE")), true);
                        payment.Status = (BillingStatus)Enum.Parse(typeof(BillingStatus), reader.GetString(reader.GetOrdinal("STATUS")), true);

                    }
                }
            }

            return payment;
        }

        public Payment ReadById(long id)
        {
            string query = $"SELECT * FROM {_tableName} WHERE ID = @Id";

            Payment payment = null;

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        payment = new Payment
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            PaidAmount = reader.GetDouble(reader.GetOrdinal("PAID_AMOUNT")),
                            PaidDate = reader.GetString(reader.GetOrdinal("PAID_DATE")),
                            BalanceAmount = reader.GetDouble(reader.GetOrdinal("BALANCE_AMOUNT")),
                            BalancePaidDate = reader.GetString(reader.GetOrdinal("BALANCE_PAID_DATE"))
                        };
                        payment.Billing = _billingDao.Read(reader.GetInt64(reader.GetOrdinal("Billing_ID")));
                        payment.PaymentMode = (PaymentMode)Enum.Parse(typeof(PaymentMode), reader.GetString(reader.GetOrdinal("PAYMENT_MODE")), true);
                        payment.Status = (BillingStatus)Enum.Parse(typeof(BillingStatus), reader.GetString(reader.GetOrdinal("STATUS")), true);
                    }
                }
            }

            return payment;
        }

        public bool Update(Payment payment)
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {
                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Insert a single record
                    command.CommandText = $"UPDATE {_tableName} SET PAYMENT_MODE=@PaymentMode, STATUS=@Status, PAID_AMOUNT=@PaidAmt, PAID_DATE=@PaidDate, BALANCE_AMOUNT=@BalanceAmt, BALANCE_PAID_DATE=@BalancePaidDate " +
                    	$"WHERE ID = @Id";
                    command.Parameters.AddWithValue("@PaymentMode", payment.PaymentMode);
                    command.Parameters.AddWithValue("@Status", payment.Status);
                    command.Parameters.AddWithValue("@PaidAmt", payment.PaidAmount);
                    command.Parameters.AddWithValue("@PaidDate", payment.PaidDate);
                    command.Parameters.AddWithValue("@BalanceAmt", payment.BalanceAmount);
                    command.Parameters.AddWithValue("@BalancePaidDate", payment.BalancePaidDate);
                    command.Parameters.AddWithValue("@Id", payment.Id);

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
