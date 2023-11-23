using System;
namespace StoreBillingSystem.Entity
{
    public class Product
    {
        public Product()
        {
        }

        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }

        public string ProductName { get; set; }
        public Category Category { get; set; }
        public ProductType ProductType { get; set; }
        public float Qty { get; set; }
        public double PurchasePrice { get; set; }
        public float PurchaseCGSTInPercent { get; set; }
        public float PurchaseSGSTInPercent { get; set; }

        public double SellingPrice_A { get; set; }
        public double SellingPrice_B { get; set; }
        public double SellingPrice_C { get; set; }
        public double SellingPrice_D { get; set; }

        public double DiscountPrice_A { get; set; }
        public double DiscountPrice_B { get; set; }
        public double DiscountPrice_C { get; set; }
        public double DiscountPrice_D { get; set; }

        public float CGSTInPercent { get; set; }
        public float SGSTInPercent { get; set; }

        public DateTime MfgDate { get; set; }
        public DateTime ExpDate { get; set; }

        public string BatchNumber { get; set; }

    }
}
