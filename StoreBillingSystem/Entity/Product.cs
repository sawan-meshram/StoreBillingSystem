using System;
using System.Collections.Generic;
namespace StoreBillingSystem.Entity
{
    public class Product
    {
        public Product()
        {

        }

        public Product(string name, Category category, ProductType productType, float qty)
        {
            Name = name;
            Category = category;
            ProductType = productType;
            TotalQty = qty;
        }

        public long Id { get; set; }

        public string Name { get; set; }
        public Category Category { get; set; }
        public ProductType ProductType { get; set; }

        public float TotalQty { get; set; }

    }
}
