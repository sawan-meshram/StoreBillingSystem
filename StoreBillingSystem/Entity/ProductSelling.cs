using System;
namespace StoreBillingSystem.Entity
{
    public class ProductSelling
    {
        public ProductSelling()
        {
        }

        public ProductSelling(Product product)
        {
            Product = product;
        }

        public ProductSelling(Product product, float sellingCGSTInPercent, float sellingSGSTInPercent)
        {
            Product = product;
            GST(sellingCGSTInPercent, sellingSGSTInPercent);
        }

        public ProductSelling(Product product, double sellingPriceA, double discountPriceA, double sellingPriceB, double discountPriceB,
            double sellingPriceC, double discountPriceC, double sellingPriceD, double discountPriceD)
        {
            Product = product;
            SellingPriceDiscount(sellingPriceA, discountPriceA, sellingPriceB, discountPriceB, sellingPriceC, discountPriceC, sellingPriceD, discountPriceD);
        }

        public ProductSelling(Product product, double sellingPriceA, double discountPriceA, double sellingPriceB, double discountPriceB,
            double sellingPriceC, double discountPriceC, double sellingPriceD, double discountPriceD,
            float sellingCGSTInPercent, float sellingSGSTInPercent)
        {
            Product = product;
            SellingPriceDiscount(sellingPriceA, discountPriceA, sellingPriceB, discountPriceB, sellingPriceC, discountPriceC, sellingPriceD, discountPriceD);
            GST(sellingCGSTInPercent, sellingSGSTInPercent);
        }

        public long Id { get; set; }
        public float CGSTInPercent { get; set; }
        public float SGSTInPercent { get; set; }

        public double SellingPrice_A { get; set; }
        public double SellingPrice_B { get; set; }
        public double SellingPrice_C { get; set; }
        public double SellingPrice_D { get; set; }

        public double DiscountPrice_A { get; set; }
        public double DiscountPrice_B { get; set; }
        public double DiscountPrice_C { get; set; }
        public double DiscountPrice_D { get; set; }

        public Product Product { get; set; }

        public void GST(float sellingCGSTInPercent, float sellingSGSTInPercent)
        {
            CGSTInPercent = sellingCGSTInPercent;
            SGSTInPercent = sellingSGSTInPercent;
        }

        public void SellingPriceDiscount(double sellingPriceA, double discountPriceA, double sellingPriceB, double discountPriceB, 
            double sellingPriceC, double discountPriceC, double sellingPriceD, double discountPriceD)
        {
            SellingPriceDiscount_A(sellingPriceA, discountPriceA);
            SellingPriceDiscount_B(sellingPriceB, discountPriceB);
            SellingPriceDiscount_C(sellingPriceC, discountPriceC);
            SellingPriceDiscount_D(sellingPriceD, discountPriceD);
        }

        public void SellingPriceDiscount_A(double sellingPrice, double discountPrice)
        {
            SellingPrice_A = sellingPrice;
            DiscountPrice_A = discountPrice;
        }

        public void SellingPriceDiscount_B(double sellingPrice, double discountPrice)
        {
            SellingPrice_B = sellingPrice;
            DiscountPrice_B = discountPrice;
        }

        public void SellingPriceDiscount_C(double sellingPrice, double discountPrice)
        {
            SellingPrice_C = sellingPrice;
            DiscountPrice_C = discountPrice;
        }
        public void SellingPriceDiscount_D(double sellingPrice, double discountPrice)
        {
            SellingPrice_D = sellingPrice;
            DiscountPrice_D = discountPrice;
        }

    }
}
