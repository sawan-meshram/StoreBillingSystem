using System;
using System.Collections.Generic;
using StoreBillingSystem.Entity;

namespace StoreBillingSystem.DAO
{
    public interface IProductPurchaseDao
    {
        IList<ProductPurchase> ReadAll();
        ProductPurchase Read(int id);
        bool Insert(ProductPurchase category);
        bool Update(ProductPurchase category);
        bool Delete(int id);
    }
}
