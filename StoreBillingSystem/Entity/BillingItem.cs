using System;
using StoreBillingSystem.Util;
namespace StoreBillingSystem.Entity
{
    public class BillingItem
    {
        public BillingItem()
        {
        }

        public BillingItem(ProductSelling selling)
        {
            ProductSelling = selling;
        }

        public BillingItem(ProductSelling selling, SellingType type) : this(selling)
        {
            Type = type;
        }


        public BillingItem(ProductSelling selling, SellingType type, float askQty, double askAmt) : this(selling, type)
        {
            AskQty = askQty;
            AskAmount = askAmt;
        }

        public BillingItem(int srNo, ProductSelling selling, SellingType type, float askQty, double askAmt) : this(selling, type, askQty, askAmt)
        {
            SrNo = srNo;
        }

        public int SrNo { get; set; }
        public ProductSelling ProductSelling { get; set; }

        public SellingType Type { get; set; }

        public float AskQty { get; set; }
        public double AskAmount { get; set; }

        public double SellingPrice
        {
            get
            {
                switch (Type)
                {
                    case SellingType.A: return ProductSelling.SellingPrice_A;
                    case SellingType.B: return ProductSelling.SellingPrice_B;
                    case SellingType.C: return ProductSelling.SellingPrice_C;
                    case SellingType.D: return ProductSelling.SellingPrice_D;
                }
                return 0;
            }
        }

        public double DiscountPrice
        {
            get
            {
                switch (Type)
                {
                    case SellingType.A: return ProductSelling.DiscountPrice_A;
                    case SellingType.B: return ProductSelling.DiscountPrice_B;
                    case SellingType.C: return ProductSelling.DiscountPrice_C;
                    case SellingType.D: return ProductSelling.DiscountPrice_D;
                }
                return 0;
            }
        }

        public double TotalAmount
        {
            get
            {
                return SellingPrice * AskQty;
            }
        }

        public double TotalDiscount
        {
            get
            {
                return DiscountPrice * AskQty;
            }
        }

        public double Total
        {
            get
            {
                return TotalAmount - TotalDiscount;
            }
        }

        /*
        public float GST_Percent { get; set; }
        public float GST_Rs { get; set; }
        public float Discount_Percent { get; set; }
        public float Discount { get; set; }
        public double Total { get; set; }
        */
    }
}
