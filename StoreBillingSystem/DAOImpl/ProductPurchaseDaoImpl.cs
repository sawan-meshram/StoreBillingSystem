using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;


using StoreBillingSystem.DAO;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Util;

namespace StoreBillingSystem.DAOImpl
{
    public class ProductPurchaseDaoImpl : IProductPurchaseDao
    {
        private SqliteConnection _conn;
        private string _tableName;
        private IProductDao _productDao;

        public ProductPurchaseDaoImpl(SqliteConnection conn)
        {
            _conn = conn;
            _tableName = StoreDbTable.ProductPurchase.ToString();
            _productDao =new ProductDaoImpl(conn);
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

        public bool Insert(ProductPurchase purchase)
        {
            string query = $"INSERT INTO {_tableName} (PURCHASE_DATE, QTY, PRICE, CGST_PERCENT, SGST_PERCENT, MFG_DATE, EXP_DATE, BATCH, Product_ID) " +
            	$"VALUES (@PurchaseDate, @Qty, @PurchasePrice, @CGST, @SGST, @Mfg, @Exp, @Batch, @ProductId); SELECT last_insert_rowid()";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Insert a single record
                    command.Parameters.AddWithValue("@PurchaseDate", purchase.PurchaseDate);
                    command.Parameters.AddWithValue("@Qty", purchase.Qty);
                    command.Parameters.AddWithValue("@PurchasePrice", purchase.PurchasePrice);
                    command.Parameters.AddWithValue("@CGST", purchase.PurchaseCGSTInPercent);
                    command.Parameters.AddWithValue("@SGST", purchase.PurchaseSGSTInPercent);
                    command.Parameters.AddWithValue("@Mfg", purchase.MfgDate);
                    command.Parameters.AddWithValue("@Exp", purchase.ExpDate);
                    command.Parameters.AddWithValue("@Batch", purchase.BatchNumber);
                    command.Parameters.AddWithValue("@ProductId", purchase.Product.Id);

                    long generatedId = (long)command.ExecuteScalar();
                    Console.WriteLine($"Auto-generated ID: {generatedId}");

                    purchase.Id = generatedId;

                    transaction.Commit();
                    return true;
                }
            }
        }

        public ProductPurchase Read(long id)
        {
            string query = $"SELECT * FROM {_tableName} WHERE ID = @Id";

            ProductPurchase purchase = null;

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        purchase = new ProductPurchase()
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            PurchaseDate = reader.GetString(reader.GetOrdinal("PURCHASE_DATE")),
                            Qty = reader.GetFloat(reader.GetOrdinal("QTY")),
                            PurchasePrice = reader.GetDouble(reader.GetOrdinal("PRICE")),
                            PurchaseCGSTInPercent = reader.GetFloat(reader.GetOrdinal("CGST_PERCENT")),
                            PurchaseSGSTInPercent = reader.GetFloat(reader.GetOrdinal("SGST_PERCENT")),
                            MfgDate = reader.GetString(reader.GetOrdinal("MFG_DATE")),
                            ExpDate = reader.GetString(reader.GetOrdinal("EXP_DATE")),
                            BatchNumber = reader.GetString(reader.GetOrdinal("BATCH")),
                        };

                        purchase.Product = _productDao.Read(reader.GetInt64(reader.GetOrdinal("Product_ID")));
                    }
                }
            }

            return purchase;
        }

        public IList<ProductPurchase> Read(Product product)
        {
            string query = $"SELECT * FROM {_tableName} WHERE Product_ID = @ProductId";

            IList<ProductPurchase> purchases = new List<ProductPurchase>();

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@ProductId", product.Id);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        purchases.Add(new ProductPurchase()
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            PurchaseDate = reader.GetString(reader.GetOrdinal("PURCHASE_DATE")),
                            Qty = reader.GetFloat(reader.GetOrdinal("QTY")),
                            PurchasePrice = reader.GetDouble(reader.GetOrdinal("PRICE")),
                            PurchaseCGSTInPercent = reader.GetFloat(reader.GetOrdinal("CGST_PERCENT")),
                            PurchaseSGSTInPercent = reader.GetFloat(reader.GetOrdinal("SGST_PERCENT")),
                            MfgDate = reader.GetString(reader.GetOrdinal("MFG_DATE")),
                            ExpDate = reader.GetString(reader.GetOrdinal("EXP_DATE")),
                            BatchNumber = reader.GetString(reader.GetOrdinal("BATCH")),
                            Product = product
                        });
                    }
                }
            }

            return purchases;
        }

        public IList<ProductPurchase> ReadAll()
        {
            IList<ProductPurchase> purchases = new List<ProductPurchase>();

            string query = $"SELECT * FROM {_tableName}";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProductPurchase purchase = new ProductPurchase()
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            PurchaseDate = reader.GetString(reader.GetOrdinal("PURCHASE_DATE")),
                            Qty = reader.GetFloat(reader.GetOrdinal("QTY")),
                            PurchasePrice = reader.GetDouble(reader.GetOrdinal("PRICE")),
                            PurchaseCGSTInPercent = reader.GetFloat(reader.GetOrdinal("CGST_PERCENT")),
                            PurchaseSGSTInPercent = reader.GetFloat(reader.GetOrdinal("SGST_PERCENT")),
                            MfgDate = reader.GetString(reader.GetOrdinal("MFG_DATE")),
                            ExpDate = reader.GetString(reader.GetOrdinal("EXP_DATE")),
                            BatchNumber = reader.GetString(reader.GetOrdinal("BATCH")),
                        };
                        purchase.Product = _productDao.Read(reader.GetInt64(reader.GetOrdinal("Product_ID")));

                        purchases.Add(purchase);
                    }
                }
            }

            return purchases;
        }

        public bool Update(ProductPurchase purchase)
        {
            string query = $"UPDATE {_tableName} " +
            	"SET PURCHASE_DATE=@PurchaseDate, QTY=@Qty, PRICE=@PurchasePrice, CGST_PERCENT=@CGST, SGST_PERCENT=@SGST, MFG_DATE=@Mfg, EXP_DATE=@Exp, BATCH=@Batch, Product_ID=@ProductId " +
                "WHERE ID = @Id";
            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {

                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    command.Parameters.AddWithValue("@PurchaseDate", purchase.PurchaseDate);
                    command.Parameters.AddWithValue("@Qty", purchase.Qty);
                    command.Parameters.AddWithValue("@PurchasePrice", purchase.PurchasePrice);
                    command.Parameters.AddWithValue("@CGST", purchase.PurchaseCGSTInPercent);
                    command.Parameters.AddWithValue("@SGST", purchase.PurchaseSGSTInPercent);
                    command.Parameters.AddWithValue("@Mfg", purchase.MfgDate);
                    command.Parameters.AddWithValue("@Exp", purchase.ExpDate);
                    command.Parameters.AddWithValue("@Batch", purchase.BatchNumber);
                    command.Parameters.AddWithValue("@ProductId", purchase.Product.Id);
                    command.Parameters.AddWithValue("@Id", purchase.Id);

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
