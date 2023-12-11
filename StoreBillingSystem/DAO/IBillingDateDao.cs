
using System.Collections.Generic;
using StoreBillingSystem.Entity;
namespace StoreBillingSystem.DAO
{
    public interface IBillingDateDao
    {
        IList<BillingDate> ReadAll();
        BillingDate Read(long id);
        BillingDate Read(string billingDate);
        bool Insert(BillingDate billingDate);
        bool Delete(long id);
        bool IsRecordExists(string billingDate);
    }
}
