using System;
using System.Collections.Generic;
using StoreBillingSystem.Entity;

namespace StoreBillingSystem.DAO
{
    public interface IProductPurchaseDao
    {
        IList<ProductPurchase> ReadAll();
        ProductPurchase Read(long id);
        IList<ProductPurchase> Read(Product product);
        bool Insert(ProductPurchase purchase);
        bool Update(ProductPurchase purchase);
        bool Delete(long id);
    }
}
