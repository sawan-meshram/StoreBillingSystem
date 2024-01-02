using System;
namespace StoreBillingSystem.Entity
{
    public class ProductPurchase
    {
        public ProductPurchase()
        {
        }

        public ProductPurchase(Product product)
        {
            Product = product;
        }

        public ProductPurchase(Product product, float qty, double purchasePrice, string purchaseDate) : this(product)
        {
            Qty = qty;
            PurchasePrice = purchasePrice;
            PurchaseDate = purchaseDate;
        }

        public ProductPurchase(Product product, float qty, double purchasePrice, float purchaseCGSTInPercent, float purchaseSGSTInPercent, 
            string purchaseDate) : this (product, qty, purchasePrice, purchaseDate)
        {
            GST(purchaseCGSTInPercent, purchaseSGSTInPercent);
        }

        public Product Product { get; set; }

        public long Id { get; set; }
        public string PurchaseDate { get; set; }
        public float Qty { get; set; }
        public double PurchasePrice { get; set; }
        public float PurchaseCGSTInPercent { get; set; }
        public float PurchaseSGSTInPercent { get; set; }

        public string MfgDate { get; set; }
        public string ExpDate { get; set; }

        public string BatchNumber { get; set; }

        public DateTime PurchaseDateTime
        {
            get { 
                return DateTime.ParseExact(PurchaseDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        public void GST(float purchaseCGSTInPercent, float purchaseSGSTInPercent)
        {
            PurchaseCGSTInPercent = purchaseCGSTInPercent;
            PurchaseSGSTInPercent = purchaseSGSTInPercent;
        }

        public void MfgExpBatch(string mfgDate, string expDate, string batchNumber)
        {
            MfgDate = mfgDate;
            ExpDate = expDate;
            BatchNumber = batchNumber;
        }

        /// <summary>
        /// This calculate the total purchase price (Include GST).
        /// </summary>
        /// <returns>The total price.</returns>
        public double GetTotalPurchasePrice()
        {
            return PurchasePrice * Qty;
        }

        /// <summary>
        /// This calculates the total gst in percentage.
        /// </summary>
        /// <returns>The total GST in Percentage</returns>
        public float GetTotalGSTInPercent()
        {
            return PurchaseCGSTInPercent + PurchaseSGSTInPercent;
        }

        /// <summary>
        /// This calculate the total tax gst price.
        /// </summary>
        /// <returns>Total purchase GST price</returns>
        public double GetTotalTaxablePurchaseGSTInPrice()
        {
            //Tax Value= (Total Value / 1 + Tax Rate) * Tax Rate
            //OR
            //Tax Value = Total Value - (Total Value / (1 + Tax Rate))
            return GetTotalPurchasePrice() - (GetTotalPurchasePrice() / (1 + (GetTotalGSTInPercent() / 100)));
        }

        /// <summary>
        /// This calculates the total taxable purchase price (exclude GST).
        /// </summary>
        /// <returns>Total purchase price exclude GST in it.</returns>
        public double GetTotalTaxablePurchasePrice()
        {
            return GetTotalPurchasePrice() - GetTotalTaxablePurchaseGSTInPrice();
        }


       

    }
}
