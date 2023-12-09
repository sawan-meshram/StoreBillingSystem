using System;
using System.Collections.Generic;
namespace StoreBillingSystem.Entity
{
    public class BillingDetails
    {
        public BillingDetails()
        {
        }
        public BillingDetails(long id)
        {
            Id = id;
        }

        public BillingDetails(Billing billing)
        {
            Billing = billing;
        }

        public BillingDetails(long id, Billing billing) : this(billing)
        {
            Id = id;
        }

        public BillingDetails(long id, Billing billing, IList<Item> items): this(id, billing)
        {
            Items = items;
        }

        public long Id { get; set; }
        public Billing Billing { get; set; }

        public IList<Item> Items { get; set; }


        public override string ToString()
        {
            return $"Item : Id={Id}, {Billing}, {Items}";
        }
    }
}
