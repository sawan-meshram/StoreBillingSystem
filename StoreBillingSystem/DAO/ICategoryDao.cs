using System.Collections.Generic;

using StoreBillingSystem.Entity;
namespace StoreBillingSystem.DAO
{
    public interface ICategoryDao
    {
        IList<Category> ReadAll();
        Category Read(int id);
        bool Insert(Category category);
        bool Update(Category category);
        bool Delete(int id);
        bool IsRecordExists(string name);
    }
}
