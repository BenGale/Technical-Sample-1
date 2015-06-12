using System;
using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Items;
using BasketTest.Discounts.VoucherValidation;

namespace BasketTest.Discounts
{
    public class Basket
    {
        private readonly IVoucherValidator _voucherValidator;
        public List<Product> Products { get; }
        public List<GiftVoucher> Vouchers { get; }
        public List<string> StatusMessages { get; set; }
        public List<InvalidVoucher> InvalidVouchers { get; }

        public Basket(IVoucherValidator voucherValidator)
        {
            _voucherValidator = voucherValidator;

            Products = new List<Product>();
            Vouchers = new List<GiftVoucher>();
            InvalidVouchers = new List<InvalidVoucher>();
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
            InvalidVouchers.AddRange(_voucherValidator.Validate(Products, Vouchers));
            RemoveInvalidVouchers();
        }

        public void RemoveVoucher(GiftVoucher voucher)
        {
            Vouchers.Remove(voucher);
        }

        public decimal Total()
        {
            var productTotal = Products.Sum(product => product.Value);
            var voucherTotal = Vouchers.Sum(voucher => voucher.Value);

            return productTotal - voucherTotal;
        }

        private void RemoveInvalidVouchers()
        {
            Vouchers.RemoveAll(voucher => InvalidVouchers.Any(iv => iv.Voucher == voucher));
        }
    }
}