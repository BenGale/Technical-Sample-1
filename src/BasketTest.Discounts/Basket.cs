using System;
using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Items;

namespace BasketTest.Discounts
{
    public class Basket
    {
        public List<Product> Products { get; }
        public List<GiftVoucher> Vouchers { get; }
        public List<string> StatusMessages { get; set; }

        public Basket()
        {
            Products = new List<Product>();
            Vouchers = new List<GiftVoucher>();
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }
        public void RemoveProduct(Product product)
        {
            Products.Remove(product);
        }

        public void AddVoucher(GiftVoucher voucher)
        {
            Vouchers.Add(voucher);
        }

        public void RemoveVoucher(GiftVoucher voucher)
        {
            Vouchers.Remove(voucher);
        }

        public decimal Total()
        {
            var productTotal = Products.Sum(product => product.Value);
            var voucherTotal = Vouchers.Sum(voucher => voucher.Value);

            return productTotal + voucherTotal;
        }
    }
}