using System;
using System.Collections.Generic;
using StoreBillingSystem.Entity;
namespace StoreBillingSystem.DAO
{
    public interface IBillingDetailsDao
    {
        BillingDetails Read(long billingId);
        BillingDetails Read(Billing billing);
        bool Insert(BillingDetails details);
    }
}
