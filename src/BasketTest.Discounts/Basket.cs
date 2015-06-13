using System;
using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Items;
using BasketTest.Discounts.VoucherValidation;

namespace BasketTest.Discounts
{
    public class Basket
    {
        private readonly IGiftVoucherValidator _giftVoucherValidator;
        public List<Product> Products { get; }
        public List<GiftVoucher> GiftVouchers { get; }
        public List<InvalidVoucher> InvalidVouchers { get; }
        public OfferVoucher OfferVoucher { get; set; }

        public Basket(IGiftVoucherValidator giftVoucherValidator)
        {
            _giftVoucherValidator = giftVoucherValidator;

            Products = new List<Product>();
            GiftVouchers = new List<GiftVoucher>();
            InvalidVouchers = new List<InvalidVoucher>();
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
            ValidateBasket();
        }
        public void RemoveProduct(Product product)
        {
            Products.Remove(product);
            ValidateBasket();
        }

        public void AddGiftVoucher(GiftVoucher voucher)
        {
            GiftVouchers.Add(voucher);
            ValidateBasket();
        }

        public void RemoveVoucher(GiftVoucher voucher)
        {
            GiftVouchers.Remove(voucher);
            InvalidVouchers.RemoveAll(invalidVoucher => invalidVoucher.Voucher == voucher);
            ValidateBasket();
        }

        public void AddOfferVoucher(OfferVoucher offerVoucher)
        {
            OfferVoucher = offerVoucher;
        }

        public decimal Total()
        {
            var productTotal = Products.Sum(product => product.Value);
            var voucherTotal = GiftVouchers.Sum(voucher => voucher.Value);

            return productTotal - voucherTotal;
        }

        private void ValidateBasket()
        {
            // Here we want to reaccess the basket as if all vouchers are valid
            // and see what the validators decide now that the variables have
            // changed.
            GiftVouchers.AddRange(InvalidVouchers.Select(v => v.Voucher));
            InvalidVouchers.Clear();
            InvalidVouchers.AddRange(_giftVoucherValidator.Validate(Products, GiftVouchers));
            GiftVouchers.RemoveAll(voucher => InvalidVouchers.Any(iv => iv.Voucher == voucher));
        }
    }
}