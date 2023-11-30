using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;

using StoreBillingSystem.DAO;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Util;

namespace StoreBillingSystem.DAOImpl
{
    public class ProductDaoImpl : IProductDao
    {
        private SqliteConnection _conn;
        private string _tableName;
        //private string _refCategoryTableName;
        //private string _refProductTypeTableName;
        private ICategoryDao _categoryDao;
        private IProductTypeDao _productTypeDao;

        public ProductDaoImpl(SqliteConnection conn)
        {
            _conn = conn;
            _tableName = StoreDbTable.Product.ToString();
            //_refCategoryTableName = StoreDbTable.Category.ToString();
            //_refProductTypeTableName = StoreDbTable.ProductType.ToString();
            _categoryDao = new CategoryDaoImpl(conn);
            _productTypeDao = new ProductTypeDaoImpl(conn);
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

        public long GetNewProductId()
        {
            string query = $"SELECT MAX(ID) FROM {_tableName}";
            long newProductId = 0;
            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    // If there are existing ProductIds, increment the maximum one
                    newProductId = Convert.ToInt64(result) + 1;
                }
                else
                {
                    newProductId = 1;
                }
            }
            return newProductId;
        }

        public bool Insert(Product product)
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {

                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Insert a single record
                    command.CommandText = $"INSERT INTO {_tableName} (ID, NAME, QTY, Category_ID, ProductType_ID) VALUES (@Id, @Name, @Qty, @CategoryId, @ProductTypeId); SELECT last_insert_rowid()";
                    command.Parameters.AddWithValue("@Id", product.Id);
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Qty", product.TotalQty);
                    command.Parameters.AddWithValue("@CategoryId", product.Category.Id);
                    command.Parameters.AddWithValue("@ProductTypeId", product.ProductType.Id);

                    long generatedId = (long)command.ExecuteScalar();
                    Console.WriteLine($"Auto-generated ID: {generatedId}");

                    product.Id = generatedId;

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

        public IList<string> ProductNames()
        {
            IList<string> productNames = new List<string>();

            string query = $"SELECT NAME FROM {_tableName}";

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productNames.Add(reader.GetString(reader.GetOrdinal("NAME")));
                    }
                }
            }
            return productNames;
        }

        public Product Read(long id)
        {
            /*
            string query = $"SELECT {_tableName}.*, {_refCategoryTableName}.*, {_refProductTypeTableName}.* " +
                           $"FROM {_tableName} " +
                           $"INNER JOIN {_refCategoryTableName} ON {_tableName}.Category_ID = {_refCategoryTableName}.ID " +
                           $"INNER JOIN {_refProductTypeTableName} ON {_tableName}.ProductType_ID = {_refProductTypeTableName}.ID " +
                           $"WHERE {_tableName}.ID = @Id";
            */
            string query = $"SELECT * FROM {_tableName} WHERE ID = @Id";

            Product product = null;

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        product = new Product()
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            Name = reader.GetString(reader.GetOrdinal("NAME")),
                            TotalQty = reader.GetFloat(reader.GetOrdinal("QTY"))

                            /*
                            Id = reader.GetInt64(reader.GetOrdinal($"{_tableName}.ID")),
                            Name = reader.GetString(reader.GetOrdinal($"{_tableName}.NAME")),
                            TotalQty = reader.GetFloat(reader.GetOrdinal($"{_tableName}.QTY")),

                            Category = new Category
                            (
                                reader.GetInt32(reader.GetOrdinal($"{_refCategoryTableName}.ID")), 
                                reader.GetString(reader.GetOrdinal($"{_refCategoryTableName}.NAME"))
                            ),
                            ProductType = new ProductType
                            (
                                reader.GetInt32(reader.GetOrdinal($"{_refProductTypeTableName}.ID")),
                                reader.GetString(reader.GetOrdinal($"{_refProductTypeTableName}.NAME")),
                                reader.GetString(reader.GetOrdinal($"{_refProductTypeTableName}.ABBR"))
                            )*/
                        };
                        product.Category = _categoryDao.Read(reader.GetInt32(reader.GetOrdinal("Category_ID")));
                        product.ProductType = _productTypeDao.Read(reader.GetInt32(reader.GetOrdinal("ProductType_ID")));
                    }
                }
            }

            return product;
        }

        public IList<Product> ReadAll()
        {
            IList<Product> products = new List<Product>();

            string query = $"SELECT * FROM {_tableName}";

            /*
            string query = $"SELECT {_tableName}.*, {_refCategoryTableName}.*, {_refProductTypeTableName}.* " +
                          $"FROM {_tableName} " +
                          $"INNER JOIN {_refCategoryTableName} ON {_tableName}.Category_ID = {_refCategoryTableName}.ID " +
                          $"INNER JOIN {_refProductTypeTableName} ON {_tableName}.ProductType_ID = {_refProductTypeTableName}.ID";
            */

            using (SqliteCommand command = new SqliteCommand(query, _conn))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        /*
                        products.Add(
                            new Product(){
                                Id = reader.GetInt64(reader.GetOrdinal($"{_tableName}.ID")),
                                Name = reader.GetString(reader.GetOrdinal($"{_tableName}.NAME")),
                                TotalQty = reader.GetFloat(reader.GetOrdinal($"{_tableName}.QTY")),
                                Category = new Category(
                                    reader.GetInt32(reader.GetOrdinal($"{_refCategoryTableName}.ID")),
                                    reader.GetString(reader.GetOrdinal($"{_refCategoryTableName}.NAME"))
                                ),
                                ProductType = new ProductType(
                                    reader.GetInt32(reader.GetOrdinal($"{_refProductTypeTableName}.ID")),
                                    reader.GetString(reader.GetOrdinal($"{_refProductTypeTableName}.NAME")),
                                    reader.GetString(reader.GetOrdinal($"{_refProductTypeTableName}.ABBR"))
                                )
                            }
                        );
                        */

                        Product product = new Product()
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
                            Name = reader.GetString(reader.GetOrdinal("NAME")),
                            TotalQty = reader.GetFloat(reader.GetOrdinal("QTY"))
                        };
                       
                        product.Category = _categoryDao.Read(reader.GetInt32(reader.GetOrdinal("Category_ID")));
                        product.ProductType = _productTypeDao.Read(reader.GetInt32(reader.GetOrdinal("ProductType_ID")));

                        products.Add(product);
                    }
                }
            }
            return products;
        }

        public bool Update(Product product)
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {

                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Insert a single record
                    command.CommandText = $"UPDATE {_tableName} SET NAME=@Name, QTY=@Qty, Category_ID=@CategoryID, ProductType_ID=@ProductTypeID WHERE ID = @Id";
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Qty", product.TotalQty);
                    command.Parameters.AddWithValue("@CategoryID", product.Category.Id);
                    command.Parameters.AddWithValue("@ProductTypeID", product.ProductType.Id);
                    command.Parameters.AddWithValue("@Id", product.Id);

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

        public bool UpdateQty(long id, float qty)
        {
            using (SqliteCommand command = _conn.CreateCommand())
            {

                // Insert multiple records using a transaction
                using (SqliteTransaction transaction = _conn.BeginTransaction())
                {
                    // Insert a single record
                    command.CommandText = $"UPDATE {_tableName} SET QTY=@Qty WHERE ID = @Id";
                    command.Parameters.AddWithValue("@Qty", qty);
                    command.Parameters.AddWithValue("@Id", id);

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
