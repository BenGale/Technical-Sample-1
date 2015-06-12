using BasketTest.Discounts.Enums;

namespace BasketTest.Discounts.Items
{
    public class Product
    {
        public readonly string Name;
        public readonly decimal Value;
        public readonly ProductCategory Category;

        public Product(
            string name, decimal value, ProductCategory category = ProductCategory.Misc)
        {
            Name = name;
            Value = value;
            Category = category;
        }
    }
}