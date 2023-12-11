using System;
using System.Collections.Generic;

using StoreBillingSystem.Entity;
namespace StoreBillingSystem.DAO
{
    public interface IBillingDao
    {
        IList<Billing> ReadAll();
        Billing Read(long id);
        Billing Read(long billingNumber, BillingDate billingDate);
        bool Insert(Billing billing);
        bool Delete(long id);
        long GetNewBillingNumber(BillingDate billingDate);

    }
}
