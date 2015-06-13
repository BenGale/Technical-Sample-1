﻿using System;
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
        public List<Voucher> Vouchers { get; }
        public List<InvalidVoucher> InvalidVouchers { get; }

        public Basket(IVoucherValidator voucherValidator)
        {
            _voucherValidator = voucherValidator;

            Products = new List<Product>();
            Vouchers = new List<Voucher>();
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

        public void AddGiftVoucher(Voucher voucher)
        {
            Vouchers.Add(voucher);
            ValidateBasket();
        }

        public void RemoveVoucher(Voucher voucher)
        {
            Vouchers.Remove(voucher);
            InvalidVouchers.RemoveAll(invalidVoucher => invalidVoucher.Voucher == voucher);
            ValidateBasket();
        }

        public void AddOfferVoucher(Voucher voucher)
        {
            if (Vouchers.OfType<OfferVoucher>().Any())
            {
                InvalidVouchers.Add(new InvalidVoucher(
                    voucher, "You may only have one offer voucher in the basket."));    
                 return;
            }
            Vouchers.Add(voucher);
        }

        public decimal Total()
        {
            var productTotal = Products.Sum(product => product.Value);
            var voucherTotal = Vouchers.Sum(voucher => voucher.Value);

            return productTotal - voucherTotal;
        }

        private void ValidateBasket()
        {
            // Here we want to reaccess the basket as if all vouchers are valid
            // and see what the validators decide now that the variables have
            // changed.
            Vouchers.AddRange(InvalidVouchers.Select(v => v.Voucher));
            InvalidVouchers.Clear();
            InvalidVouchers.AddRange(_voucherValidator.Validate(Products, Vouchers));
            Vouchers.RemoveAll(voucher => InvalidVouchers.Any(iv => iv.Voucher == voucher));
        }
    }
}