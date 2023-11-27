using System;
using System.Collections.Generic;
using StoreBillingSystem.Entity;

namespace StoreBillingSystem.DAO
{
    public interface IProductSellingDao
    {
        IList<ProductSelling> ReadAll();
        ProductSelling Read(int id);
        bool Insert(ProductSelling category);
        bool Update(ProductSelling category);
        bool Delete(int id);
    }
}
