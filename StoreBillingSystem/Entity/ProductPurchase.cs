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

        public ProductPurchase(Product product, float qty, double purchasePrice, DateTime purchaseDate) 
        {
            Product = product;
            Qty = qty;
            PurchasePrice = purchasePrice;
            PurchaseDate = purchaseDate;
        }

        public ProductPurchase(Product product, float qty, double purchasePrice, float purchaseCGSTInPercent, float purchaseSGSTInPercent, DateTime purchaseDate)
        {
            Product = product;
            Qty = qty;
            PurchasePrice = purchasePrice;
            PurchaseDate = purchaseDate;
            GST(purchaseCGSTInPercent, purchaseSGSTInPercent);
        }


        public long Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public float Qty { get; set; }
        public double PurchasePrice { get; set; }
        public float PurchaseCGSTInPercent { get; set; }
        public float PurchaseSGSTInPercent { get; set; }

        public DateTime MfgDate { get; set; }
        public DateTime ExpDate { get; set; }

        public string BatchNumber { get; set; }

        public Product Product { get; set; }

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
    }
}
