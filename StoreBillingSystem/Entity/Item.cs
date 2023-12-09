using System;
namespace StoreBillingSystem.Entity
{
    public class Item
    {
        public Item()
        {
        }

        public Item(long srNum, Product product, float qty, 
            double price, double grossAmt, double gstInPercent, double discountPrice, double netAmt)
        {
            SrNum = srNum;
            Product = product;
            Qty = qty;
            Price = price;
            GrossAmount = grossAmt;
            GSTPercent = gstInPercent;
            DiscountPrice = discountPrice;
            NetAmount = netAmt;
        }

        public long SrNum { get; set; }
        public Product Product { get; set; }
        public float Qty { get; set; }
        public double Price { get; set; }
        public double GrossAmount { get; set; }
        public double GSTPercent { get; set; }
        public double DiscountPrice { get; set; }
        public double NetAmount { get; set; }

        public override string ToString()
        {
            return $"Item : SrNum={SrNum}, Qty={Qty}, Price={Price}, GrossAmount={GrossAmount}, GSTInPercent={GSTPercent}, " +
                $"DiscountInPrice={DiscountPrice}, NetAmount={NetAmount}, {Product}";
        }
    }
}
