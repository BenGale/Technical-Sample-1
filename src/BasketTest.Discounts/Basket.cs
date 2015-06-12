using System;
using System.Collections.Generic;

namespace BasketTest.Discounts
{
    public class Basket
    {
        public List<Product> Products { get; }

        public Basket()
        {
            Products = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }

        public decimal Total()
        {
            throw new NotImplementedException();
        }
    }
}