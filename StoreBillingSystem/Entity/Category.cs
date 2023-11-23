using System;
namespace StoreBillingSystem.Entity
{
    public class Category
    {
        public Category()
        {
        }
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Category : Id={Id}, Name={Name}";
        }
    }
}
