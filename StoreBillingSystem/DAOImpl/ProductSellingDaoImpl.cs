using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;

using StoreBillingSystem.DAO;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Util;

namespace StoreBillingSystem.DAOImpl
{
    public class ProductSellingDaoImpl : IProductSellingDao
    {
        private SqliteConnection _conn;
        private string _tableName;
        private IProductDao _productDao;

        public ProductSellingDaoImpl(SqliteConnection conn)
        {
            _conn = conn;
            _tableName = StoreDbTable.ProductSelling.ToString();
            _productDao = new ProductDaoImpl(conn);
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

        public bool Insert(ProductSelling selling)
        {
            string query = $"INSERT INTO {_tableName} (PRICE_A, DISCOUNT_PRICE_A, PRICE_B, DISCOUNT_PRICE_B, PRICE_C, DISCOUNT_PRICE_C, PRICE_D, DISCOUNT_PRICE_D, CGST_PERCENT, SGST_PERCENT, Product_ID) " +
                $"VALUES (@PriceA, @DiscountA, @PriceB, @DiscountB, @PriceC, @DiscountC, @PriceD, @DiscountD, @CGST, @SGST, @ProductId); SELECT last_insert_rowid()";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Insert a single record
                    command.Parameters.AddWithValue("@PriceA", selling.SellingPrice_A);
                    command.Parameters.AddWithValue("@DiscountA", selling.DiscountPrice_A);
                    command.Parameters.AddWithValue("@PriceB", selling.SellingPrice_B);
                    command.Parameters.AddWithValue("@DiscountB", selling.DiscountPrice_B);
                    command.Parameters.AddWithValue("@PriceC", selling.SellingPrice_C);
                    command.Parameters.AddWithValue("@DiscountC", selling.DiscountPrice_C);
                    command.Parameters.AddWithValue("@PriceD", selling.SellingPrice_D);
                    command.Parameters.AddWithValue("@DiscountD", selling.DiscountPrice_D);

                    command.Parameters.AddWithValue("@CGST", selling.CGSTInPercent);
                    command.Parameters.AddWithValue("@SGST", selling.SGSTInPercent);
                    command.Parameters.AddWithValue("@ProductId", selling.Product.Id);

                    long generatedId = (long)command.ExecuteScalar();
                    Console.WriteLine($"Auto-generated ID: {generatedId}");

                    selling.Id = generatedId;

                    transaction.Commit();
                    return true;
                }
            }
        }

        public ProductSelling Read(long id)
        {
            string query = $"SELECT * FROM {_tableName} WHERE ID = @Id";

            ProductSelling selling = null;
            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        selling = new ProductSelling
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            SellingPrice_A = reader.GetDouble(reader.GetOrdinal("PRICE_A")),
                            SellingPrice_B = reader.GetDouble(reader.GetOrdinal("PRICE_B")),
                            SellingPrice_C = reader.GetDouble(reader.GetOrdinal("PRICE_C")),
                            SellingPrice_D = reader.GetDouble(reader.GetOrdinal("PRICE_D")),
                            DiscountPrice_A = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE_A")),
                            DiscountPrice_B = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE_B")),
                            DiscountPrice_C = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE_C")),
                            DiscountPrice_D = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE_D")),
                            CGSTInPercent = reader.GetFloat(reader.GetOrdinal("CGST_PERCENT")),
                            SGSTInPercent = reader.GetFloat(reader.GetOrdinal("SGST_PERCENT"))
                        };

                        selling.Product = _productDao.Read(reader.GetInt64(reader.GetOrdinal("Product_ID")));
                    }
                }
            }

            return selling;
        }

        public IList<ProductSelling> ReadAll()
        {
            IList<ProductSelling> sellings = new List<ProductSelling>();

            string query = $"SELECT * FROM {_tableName}";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProductSelling selling = new ProductSelling
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            SellingPrice_A = reader.GetDouble(reader.GetOrdinal("PRICE_A")),
                            SellingPrice_B = reader.GetDouble(reader.GetOrdinal("PRICE_B")),
                            SellingPrice_C = reader.GetDouble(reader.GetOrdinal("PRICE_C")),
                            SellingPrice_D = reader.GetDouble(reader.GetOrdinal("PRICE_D")),
                            DiscountPrice_A = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE_A")),
                            DiscountPrice_B = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE_B")),
                            DiscountPrice_C = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE_C")),
                            DiscountPrice_D = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE_D")),
                            CGSTInPercent = reader.GetFloat(reader.GetOrdinal("CGST_PERCENT")),
                            SGSTInPercent = reader.GetFloat(reader.GetOrdinal("SGST_PERCENT"))
                        };

                        selling.Product = _productDao.Read(reader.GetInt64(reader.GetOrdinal("Product_ID")));

                        sellings.Add(selling);
                    }
                }
            }

            return sellings;
        }

        public ProductSelling Read(Product product)
        {
            string query = $"SELECT * FROM {_tableName} WHERE Product_ID = @ProductId";

            ProductSelling selling = null;
            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@ProductId", product.Id);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        selling = new ProductSelling
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            SellingPrice_A = reader.GetDouble(reader.GetOrdinal("PRICE_A")),
                            SellingPrice_B = reader.GetDouble(reader.GetOrdinal("PRICE_B")),
                            SellingPrice_C = reader.GetDouble(reader.GetOrdinal("PRICE_C")),
                            SellingPrice_D = reader.GetDouble(reader.GetOrdinal("PRICE_D")),
                            DiscountPrice_A = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE_A")),
                            DiscountPrice_B = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE_B")),
                            DiscountPrice_C = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE_C")),
                            DiscountPrice_D = reader.GetDouble(reader.GetOrdinal("DISCOUNT_PRICE_D")),
                            CGSTInPercent = reader.GetFloat(reader.GetOrdinal("CGST_PERCENT")),
                            SGSTInPercent = reader.GetFloat(reader.GetOrdinal("SGST_PERCENT")),
                            Product = product
                        };
                    }
                }
            }

            return selling;
        }

        public bool Update(ProductSelling selling)
        {
            string query = $"UPDATE {_tableName} " +
                "SET PRICE_A=@PriceA, DISCOUNT_PRICE_A=@DiscountA, " +
                "PRICE_B=@PriceB, DISCOUNT_PRICE_B=@DiscountB, " +
                "PRICE_C=@PriceC, DISCOUNT_PRICE_C=@DiscountC, " +
                "PRICE_D=@PriceD, DISCOUNT_PRICE_D=@DiscountD, " +
                "CGST_PERCENT=@CGST, SGST_PERCENT=@SGST, Product_ID=@ProductId " +
                "WHERE ID = @Id";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {

                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    command.Parameters.AddWithValue("@PriceA", selling.SellingPrice_A);
                    command.Parameters.AddWithValue("@DiscountA", selling.DiscountPrice_A);
                    command.Parameters.AddWithValue("@PriceB", selling.SellingPrice_A);
                    command.Parameters.AddWithValue("@DiscountB", selling.DiscountPrice_A);
                    command.Parameters.AddWithValue("@PriceC", selling.SellingPrice_A);
                    command.Parameters.AddWithValue("@DiscountC", selling.DiscountPrice_A);
                    command.Parameters.AddWithValue("@PriceD", selling.SellingPrice_A);
                    command.Parameters.AddWithValue("@DiscountD", selling.DiscountPrice_A);
                    command.Parameters.AddWithValue("@CGSTP", selling.CGSTInPercent);
                    command.Parameters.AddWithValue("@SGST", selling.SGSTInPercent);
                    command.Parameters.AddWithValue("@ProductId", selling.Product.Id);
                    command.Parameters.AddWithValue("@Id", selling.Id);

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

        public bool Delete(Product product)
        {
            // Create the SQL command to delete the record
            string deleteQuery = $"DELETE FROM {_tableName} WHERE Product_ID = @ProductId";

            using (SqliteCommand command = new SqliteCommand(deleteQuery, _conn))
            {
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Add the ID parameter to the command
                    command.Parameters.AddWithValue("@ProductId", product.Id);

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
    }
}
