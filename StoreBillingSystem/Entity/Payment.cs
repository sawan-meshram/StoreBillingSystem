using System;
namespace StoreBillingSystem.Entity
{
    public class Payment
    {
        public Payment()
        {
        }

        public Payment(long id)
        {
            Id = id;
        }

        public Payment(Billing billing)
        {
            Billing = billing;
        }

        public Payment(long id, Billing billing, PaymentMode mode, BillingStatus status, double paidAmt, string paidDate, double balanceAmt, string balancePaidDate): this(id)
        {
            Billing = billing;
            PaymentMode = mode;
            Status = status;
            PaidAmount = paidAmt;
            PaidDate = paidDate;
            BalanceAmount = balanceAmt;
            BalancePaidDate = balancePaidDate;
        }


        public long Id { get; set; }
        public Billing Billing { get; set; }
        public PaymentMode PaymentMode { get; set; }
        public BillingStatus Status { get; set; }
        public double PaidAmount { get; set; }
        public string PaidDate { get; set; }

        public double BalanceAmount { get; set; }
        public string BalancePaidDate { get; set; }

        public DateTime PaidDateTime
        {
            get
            {
                return DateTime.ParseExact(PaidDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            }
        }

        public override string ToString()
        {
            return $"Payment : Id={Id}, PaymentMode={PaymentMode}, Status={Status}, PaidAmount={PaidAmount}, PaidDate={PaidDate}, BalanceAmount={BalanceAmount}, BalancePaidDate={BalancePaidDate}, {Billing}";
        }

    }
}
