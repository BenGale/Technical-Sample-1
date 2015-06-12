﻿using System.Linq;
using BasketTest.Discounts;
using BasketTest.Discounts.Items;
using FluentAssertions;
using NUnit.Framework;

namespace BasketTest.Discount.UnitTests
{
    [TestFixture]
    public class BasketSpec
    {
        private Basket _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new Basket();
        }

        [Test]
        public void Basket_AcceptsProduct()
        {
            var testProduct = new Product("Hat", 10.50m);

            _sut.AddProduct(testProduct);

            _sut.Products.Should().HaveCount(1);
            _sut.Products.Single().Should().Be(testProduct);
        }

        [Test]
        public void Basket_AcceptsMultipleProducts()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);

            _sut.Products.Should().HaveCount(2);
            _sut.Products.Should().Contain(testProductA);
            _sut.Products.Should().Contain(testProductB);
        }

        [Test]
        public void Basket_CalculatesProductsTotal()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);
            decimal result =_sut.Total();

            result.Should().Be(65.15m);
        }

        [Test]
        public void Basket_AcceptsGiftVoucher()
        {
            var testProduct = new Product("Hat", 10m);
            var testVoucher = new GiftVoucher(5m);

            _sut.AddProduct(testProduct);
            _sut.AddVoucher(testVoucher);

            _sut.Vouchers.Should().HaveCount(1);
            _sut.Vouchers.Single().Should().Be(testVoucher);
        }

        [Test]
        public void Basket_AcceptsMultipleGiftVouchers()
        {
            var testProduct = new Product("Hat", 30m);
            var testVoucherA = new GiftVoucher(5m);
            var testVoucherB = new GiftVoucher(15m);

            _sut.AddProduct(testProduct);
            _sut.AddVoucher(testVoucherA);
            _sut.AddVoucher(testVoucherB);

            _sut.Vouchers.Should().HaveCount(2);
            _sut.Vouchers.Should().Contain(testVoucherA);
            _sut.Vouchers.Should().Contain(testVoucherB);
        }

        [Test]
        public void Basket_CalulatesWithGiftVouchers()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);
            var testVoucher = new GiftVoucher(5m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);
            _sut.AddVoucher(testVoucher);

            _sut.Total().Should().Be(60.15m);
        }

        [Test]
        public void Basket_CalulatesWithMulitpleGiftVouchers()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);
            var testVoucherA = new GiftVoucher(5m);
            var testVoucherB = new GiftVoucher(10m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);
            _sut.AddVoucher(testVoucherA);
            _sut.AddVoucher(testVoucherB);

            _sut.Total().Should().Be(50.15m);
        }

        [Test]
        public void Basket_AcceptsProductRemoval()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);

            _sut.Products.Should().HaveCount(2);

            _sut.RemoveProduct(testProductA);

            _sut.Products.Should().HaveCount(1);
            _sut.Products.Should().NotContain(testProductA);
            _sut.Products.Should().Contain(testProductB);
        }

        [Test]
        public void Basket_AcceptsVoucherRemoval()
        {
            var testProduct = new Product("Hat", 30m);
            var testVoucherA = new GiftVoucher(5m);
            var testVoucherB = new GiftVoucher(15m);

            _sut.AddProduct(testProduct);
            _sut.AddVoucher(testVoucherA);
            _sut.AddVoucher(testVoucherB);

            _sut.Vouchers.Should().HaveCount(2);

            _sut.RemoveVoucher(testVoucherA);

            _sut.Vouchers.Should().HaveCount(1);
            _sut.Vouchers.Should().NotContain(testVoucherA);
            _sut.Vouchers.Should().Contain(testVoucherB);
        }

        [Test]
        public void Basket_CaluclatesProductsTotal_AfterRemoval()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);
            _sut.RemoveProduct(testProductA);
            decimal result = _sut.Total();

            result.Should().Be(54.65m);
        }

        [Test]
        public void Basket_CaluclatesTotalWithVouchers_AfterRemoval()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);
            var testVoucherA = new GiftVoucher(5m);
            var testVoucherB = new GiftVoucher(10m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);
            _sut.AddVoucher(testVoucherA);
            _sut.AddVoucher(testVoucherB);
            _sut.RemoveVoucher(testVoucherA);

            _sut.Total().Should().Be(55.15m);
        }

        [Test]
        public void Basket_CalulatesWithoutVoucherForTooLargeDiscount()
        {
            var testProduct = new Product("Hat", 10.50m);
            var testVoucher = new GiftVoucher(15m);

            _sut.AddProduct(testProduct);
            _sut.AddVoucher(testVoucher);

            _sut.Total().Should().Be(10.50m);
        }

        [Test]
        public void Basket_CreatesInvalidVoucher_ForTooLargeDiscount()
        {
            var testProduct = new Product("Hat", 25m);
            var testVoucher = new GiftVoucher(30m);

            _sut.AddProduct(testProduct);
            _sut.AddVoucher(testVoucher);

            _sut.InvalidVouchers.Should().HaveCount(1);
            var invalidVoucher = _sut.InvalidVouchers.Single();
            invalidVoucher.Voucher.Should().Be(testVoucher);
            invalidVoucher.Reason.Should().Be(
                "You have not reached the spend threshold for this voucher.");
        }
    }
}