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

        public ProductPurchase(Product product, float qty, double purchasePrice, DateTime purchaseDate) : this(product)
        {
            Qty = qty;
            PurchasePrice = purchasePrice;
            PurchaseDate = purchaseDate;
        }

        public ProductPurchase(Product product, float qty, double purchasePrice, float purchaseCGSTInPercent, float purchaseSGSTInPercent, 
            DateTime purchaseDate) : this (product, qty, purchasePrice, purchaseDate)
        {
            GST(purchaseCGSTInPercent, purchaseSGSTInPercent);
        }

        public Product Product { get; set; }

        public long Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public float Qty { get; set; }
        public double PurchasePrice { get; set; }
        public float PurchaseCGSTInPercent { get; set; }
        public float PurchaseSGSTInPercent { get; set; }

        public DateTime MfgDate { get; set; }
        public DateTime ExpDate { get; set; }

        public string BatchNumber { get; set; }


        public void GST(float purchaseCGSTInPercent, float purchaseSGSTInPercent)
        {
            PurchaseCGSTInPercent = purchaseCGSTInPercent;
            PurchaseSGSTInPercent = purchaseSGSTInPercent;
        }

        public void MfgExpBatch(DateTime mfgDate, DateTime expDate, string batchNumber)
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
            //Tax Value = Total Value - (Total Value / (1 + Tax Rate))
            return GetTotalPurchasePrice() - (GetTotalPurchasePrice() / (1 + (GetTotalGST() / 100)));
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
