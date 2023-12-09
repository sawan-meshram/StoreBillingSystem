﻿using System;
namespace StoreBillingSystem.Entity
{
    public class BillingDate
    {
        public BillingDate()
        {
        }

        public BillingDate(long id)
        {
            Id = id;
        }

        public BillingDate(string billDate)
        {
            BillDate = billDate;
        }

        public BillingDate(long id, string billDate) : this(id)
        {
            BillDate = billDate;
        }

        public long Id { get; set; }
        public string BillDate { get; set; }

        public override string ToString()
        {
            return $"BillingDate : Id={Id}, BillDate={BillDate}";
        }
    }
}
