using System;
using System.Collections.Generic;
using StoreBillingSystem.Entity;
namespace StoreBillingSystem.DAO
{
    public interface IProductDao
    {
        IList<Product> ReadAll();
        Product Read(long id);
        Product Read(string productName);
        bool Insert(Product product);
        bool Update(Product product);
        bool UpdateQty(long id, float qty);
        bool UpdateQty(IList<Product> products);
        bool Delete(long id);
        bool IsRecordExists(string name);
        long GetNewProductId();
        IList<string> ProductNames();
    }
}
