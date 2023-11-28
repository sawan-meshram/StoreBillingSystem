using System;
namespace StoreBillingSystem.Entity
{
    public class Category
    {
        public Category()
        {
        }
        public Category(string name)
        {
            Name = name;
        }
        public Category(int id, string name) : this(name)
        {
            Id = id;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Category : Id={Id}, Name={Name}";
        }
    }
}
