using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;

using StoreBillingSystem.DAO;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Util;
namespace StoreBillingSystem.DAOImpl
{
    public class BillingDetailsDaoImpl : IBillingDetailsDao
    {
        private SqliteConnection _conn;
        private readonly string _tableName;
        private readonly int batchSize = 100;

        private IBillingDao _billingDao;
        private IProductDao _productDao;

        public BillingDetailsDaoImpl(SqliteConnection conn)
        {
            _conn = conn;
            _tableName = StoreDbTable.BillingDate.ToString();

            _billingDao = new BillingDaoImpl(conn);
            _productDao = new ProductDaoImpl(conn);
        }

        public bool Insert(BillingDetails details)
        {
            SqliteTransaction transaction = null;
            try
            {
                using (SqliteCommand command = _conn.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO {_tableName} (Billing_ID, Product_ID, SR_NUM, QTY, PRICE, GROSS_AMOUNT, GST_PERCENT, DISCOUNT_PRICE, NET_AMOUNT) " +
                    	$"VALUES (@BillingId, @ProductId, @SrNum, @Qty, @Price, @GrossAmt, @GSTPercent, @DiscountPrice, @NetAmt)";

                    transaction = _conn.BeginTransaction();


                    for (int i = 0; i < details.Items.Count; i++)
                    {
                        command.Parameters.AddWithValue("@BillingId", details.Billing.Id);
                        command.Parameters.AddWithValue("@ProductId", details.Items[i].Product.Id);
                        command.Parameters.AddWithValue("@SrNum", details.Items[i].SrNum);
                        command.Parameters.AddWithValue("@Qty", details.Items[i].Qty);
                        command.Parameters.AddWithValue("@Price", details.Items[i].Price);
                        command.Parameters.AddWithValue("@GrossAmt", details.Items[i].GrossAmount);
                        command.Parameters.AddWithValue("@GSTPercent", details.Items[i].GSTPercent);
                        command.Parameters.AddWithValue("@DiscountPrice", details.Items[i].DiscountPrice);
                        command.Parameters.AddWithValue("@NetAmt", details.Items[i].NetAmount);


                        command.ExecuteNonQuery();

                        // Check if we've reached the batch limit
                        if ((i + 1) % batchSize == 0)
                        {
                            // Commit the batch and start a new transaction
                            transaction?.Commit();
                            transaction = _conn.BeginTransaction();
                        }
                    }
                }

                // Commit the final transaction
                transaction?.Commit();
                transaction?.Dispose();
                Console.WriteLine($"{details.Items.Count} records inserted successfully.");
                return true;
            }

            catch (Exception ex)
            {
                // Handle exceptions and rollback the transaction on failure
                transaction?.Rollback();
                transaction?.Dispose();
                Console.WriteLine($"Error: {ex.Message}");
            }

            return false;
        }

        public BillingDetails Read(long billingId)
        {
            return Read(_billingDao.Read(billingId));
        }

        public BillingDetails Read(Billing billing)
        {
            string query = $"SELECT * FROM {_tableName} WHERE Billing_ID = @BillingId";

            BillingDetails details = null;
            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@BillingId", billing.Id);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if(details == null)
                        {
                            details = new BillingDetails
                            {
                                Billing = billing,
                                Items = new List<Item>()
                            };
                        }

                        Item item = new Item
                        {
                            SrNum = reader.GetInt64(reader.GetOrdinal("SR_NUM")),
                            Product = _productDao.Read(reader.GetInt64(reader.GetOrdinal("Product_ID"))),
                            Qty = reader.GetFloat(reader.GetOrdinal("QTY")),
                            Price = reader.GetDouble(reader.GetOrdinal("PRICE")),
                            GrossAmount = reader.GetDouble(reader.GetOrdinal("GROSS_AMOUNT")),
                            GSTPercent = reader.GetDouble(reader.GetOrdinal("GST_PERCENT")),
                            DiscountPrice = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE")),
                            NetAmount = reader.GetDouble(reader.GetOrdinal("NET_AMOUNT"))
                        };

                        details.Items.Add(item);

                    }
                }
            }
            return details;
        }
    }
}
