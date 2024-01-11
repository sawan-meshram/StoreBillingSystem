using System;
using System.Collections.Generic;
using StoreBillingSystem.Entity;

namespace StoreBillingSystem.DAO
{
    public interface IPaymentDao
    {
        IList<Payment> ReadAll();
        IList<Payment> ReadAllByBalance();
        Payment ReadById(long id);
        Payment ReadByBillingId(long billingId);
        bool Insert(Payment payment);
        bool Update(Payment payment);
        bool Delete(long id);
        bool IsRecordExists(long billingId);
    }
}
