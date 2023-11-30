using System;
namespace StoreBillingSystem.Entity
{
    public class Customer
    {
        public Customer()
        {
        }

        public Customer(string name, string address, long phoneNumber, string registerDate)
        {
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            RegisterDate = registerDate;
        }

        public Customer(int id, string name, string address, long phoneNumber, string registerDate) : this(name, address, phoneNumber, registerDate)
        {
            Id = id;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long PhoneNumber { get; set; }
        public string RegisterDate { get; set; }

    }
}
