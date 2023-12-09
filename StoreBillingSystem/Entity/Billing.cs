using System;
namespace StoreBillingSystem.Entity
{
    public class Billing
    {
        public Billing()
        {
        }

        public Billing(long billingNumber, string billingDateTime)
        {
            BillingNumber = billingNumber;
            BillingDateTime = billingDateTime;
        }


        public Billing(long id, long billingNumber, string billingDateTime, BillingDate billingDate, Customer customer, 
            double grossAmt, double gstInPrice, double discountInPrice, double netAmt) : this (billingNumber, billingDateTime)
        {
            Id = id;
            BillingDate = billingDate;
            Customer = customer;
            GrossAmount = grossAmt;
            GSTPrice = gstInPrice;
            DiscountPrice = discountInPrice;
            NetAmount = netAmt;
        }

        public long Id { get; set; }
        public long BillingNumber { get; set; }
        public string BillingDateTime { get; set; }

        public BillingDate BillingDate { get; set; }
        public Customer Customer { get; set; }
        public double GrossAmount { get; set; }
        public double GSTPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double NetAmount { get; set; }


        public override string ToString()
        {
            return $"Billing : Id={Id}, BillingNumber={BillingNumber}, BillingDateTime={BillingDateTime}, GrossAmount={GrossAmount}, GSTInPrice={GSTPrice}, " +
                $"DiscountInPrice={DiscountPrice}, NetAmount={NetAmount}, {Customer}, {BillingDate}";
        }
    }
}
