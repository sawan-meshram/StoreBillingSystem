using System.Collections.Generic;

using StoreBillingSystem.Entity;
namespace StoreBillingSystem.DAO
{
    public interface IProductTypeDao
    {
        IList<ProductType> ReadAll(bool sort);
        ProductType Read(int id);
        bool Insert(ProductType productType);
        bool Update(ProductType productType);
        bool Delete(int id);
        bool IsRecordExists(string name, string abbr);
    }
}
