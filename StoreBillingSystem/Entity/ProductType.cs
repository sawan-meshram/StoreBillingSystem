using System;
namespace StoreBillingSystem.Entity
{
    public class ProductType
    {
        public ProductType()
        {
        }
        public ProductType(string name, string abbr)
        {
            Name = name;
            Abbr = abbr;
        }


        public ProductType(int id, string name, string abbr)
        {
            Id = id;
            Name = name;
            Abbr = abbr;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }

        public override string ToString()
        {
            return $"ProductType : Id={Id}, Name={Name}, Abbr={Abbr}";
        }
    }
}
