using System;
using System.Collections.Generic;
using StoreBillingSystem.Entity;
namespace StoreBillingSystem.DAO
{
    public interface IProductDao
    {
        IList<Product> ReadAll();
        Product Read(int id);
        bool Insert(Product product);
        bool Update(Product product);
        bool Delete(int id);
        bool IsRecordExists(string name);
    }
}
