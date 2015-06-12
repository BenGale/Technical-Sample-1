using System;
using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Items;

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
            return Products.Sum(product => product.Value);
        }

        public void AddVoucher(GiftVoucher testVoucher)
        {
            throw new NotImplementedException();
        }
    }
}