using System;
using System.Collections.Generic;
using StoreBillingSystem.Entity;
namespace StoreBillingSystem.DAO
{
    public interface ICustomerDao
    {
        IList<Customer> ReadAll();
        Customer Read(int id);
        IList<Customer> Read(string customerName);
        bool Insert(Customer customer);
        bool Update(Customer customer);
        bool Delete(int id);
        bool IsRecordExists(string name, long phoneNumber);
        bool IsRecordExists(long phoneNumber);
        int GetNewCustomerId();
        ISet<string> CustomerNames();
        IList<string> Phones();

    }
}
