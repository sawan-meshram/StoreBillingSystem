using System;
using System.Collections.Generic;
using StoreBillingSystem.Entity;

namespace StoreBillingSystem.DAO
{
    public interface IProductSellingDao
    {
        IList<ProductSelling> ReadAll();
        ProductSelling Read(long id);
        bool Insert(ProductSelling selling);
        bool Update(ProductSelling selling);
        bool Delete(long id);
    }
}
