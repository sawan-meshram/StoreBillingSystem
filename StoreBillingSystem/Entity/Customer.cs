﻿using System;
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

        public Customer(int id, string name, string address, long phoneNumber, string registerDate, string updateDate) : this(name, address, phoneNumber, registerDate)
        {
            Id = id;
            UpdateDate = updateDate;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long PhoneNumber { get; set; }
        public string RegisterDate { get; set; }
        public string UpdateDate { get; set; }

        public DateTime RegisterDateTime
        {
            get
            {
                return DateTime.ParseExact(RegisterDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            }
        }

        public override string ToString()
        {
            return $"Customer : Id={Id}, Name={Name}, Address={Address}, PhoneNumber={PhoneNumber}, RegisterDate={RegisterDate}, UpdateDate={UpdateDate}";
        }
    }
}
