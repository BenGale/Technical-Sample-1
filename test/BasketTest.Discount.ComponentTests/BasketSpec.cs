using System.Linq;
using BasketTest.Discounts;
using BasketTest.Discounts.Enums;
using BasketTest.Discounts.Items;
using BasketTest.Discounts.VoucherValidation;
using FluentAssertions;
using NUnit.Framework;

namespace BasketTest.Discount.ComponentTests
{
    [TestFixture]
    public class BasketSpec
    {
        private IGiftVoucherValidator _validator;
        private Basket _basket;

        [SetUp]
        public void Setup()
        {
            var valueValidator = new GiftVoucherValueValidator();
            _validator = new GiftVouchersNotInTotalValidator(valueValidator);
            _basket = new Basket(_validator);
        }

        [Test]
        public void Basket_CalulatesWithoutVoucherForTooLargeDiscount()
        {
            var testProduct = new Product("Hat", 10.50m);
            var testVoucher = new GiftVoucher(15m);

            _basket.AddProduct(testProduct);
            _basket.AddGiftVoucher(testVoucher);

            _basket.Total().Should().Be(10.50m);
        }

        [Test]
        public void Basket_CreatesInvalidVoucher_ForTooLargeDiscount()
        {
            var testProduct = new Product("Hat", 25m);
            var testVoucher = new GiftVoucher(30m);

            _basket.AddProduct(testProduct);
            _basket.AddGiftVoucher(testVoucher);

            _basket.InvalidVouchers.Should().HaveCount(1);
            var invalidVoucher = _basket.InvalidVouchers.Single();
            invalidVoucher.Voucher.Should().Be(testVoucher);
            invalidVoucher.Reason.Should().Be(
                "Your total must be above the voucher value, not including gift vouchers.");
        }

        [Test]
        public void Basket_CreatesInvalidVoucher_ForTooLargeDiscount_WithGiftVoucher()
        {
            var testProduct = new Product("Hat", 25m);
            var testGiftProduct = new Product("Voucher", 10m, ProductCategory.GiftVoucher);
            var testVoucher = new GiftVoucher(30m);

            _basket.AddProduct(testProduct);
            _basket.AddProduct(testGiftProduct);
            _basket.AddGiftVoucher(testVoucher);

            _basket.InvalidVouchers.Should().HaveCount(1);
            var invalidVoucher = _basket.InvalidVouchers.Single();
            invalidVoucher.Voucher.Should().Be(testVoucher);
            invalidVoucher.Reason.Should().Be(
                "Your total must be above the voucher value, not including gift vouchers.");
        }

        [Test]
        public void Basket_CreatesInvalidVoucher_WhenProductIsRemoved()
        {
            var testProductA = new Product("Hat", 10m);
            var testProductB = new Product("Hat", 10m);
            var testVoucher = new GiftVoucher(15m);

            _basket.AddProduct(testProductA);
            _basket.AddProduct(testProductB);
            _basket.AddGiftVoucher(testVoucher);

            _basket.InvalidVouchers.Should().HaveCount(0);
            
            _basket.RemoveProduct(testProductA);

            _basket.InvalidVouchers.Should().HaveCount(1);
            var invalidVoucher = _basket.InvalidVouchers.Single();
            invalidVoucher.Voucher.Should().Be(testVoucher);
            invalidVoucher.Reason.Should().Be(
                "Your total must be above the voucher value, not including gift vouchers.");
        }

        [Test]
        public void Basket_RemovesInvalidVoucher_WhenProductIsAdded()
        {
            var testProductA = new Product("Hat", 10m);
            var testProductB = new Product("Hat", 10m);
            var testVoucher = new GiftVoucher(15m);

            _basket.AddProduct(testProductA);
            _basket.AddProduct(testProductB);
            _basket.AddGiftVoucher(testVoucher);

            _basket.InvalidVouchers.Should().HaveCount(0);

            _basket.RemoveProduct(testProductA);

            _basket.InvalidVouchers.Should().HaveCount(1);

            _basket.AddProduct(testProductA);

            _basket.InvalidVouchers.Should().HaveCount(0);
        }

        [Test]
        public void Basket_RemovesInvalidVoucher_WhenVoucherIsRemoved()
        {
            var testProduct = new Product("Hat", 10m);
            var testVoucherA = new GiftVoucher(5m);
            var testVoucherB = new GiftVoucher(8m);
            var testVoucherC = new GiftVoucher(7.50m);

            _basket.AddProduct(testProduct);
            _basket.AddGiftVoucher(testVoucherA);
            _basket.AddGiftVoucher(testVoucherB);
            _basket.AddGiftVoucher(testVoucherC);

            _basket.InvalidVouchers.Should().HaveCount(2);
            _basket.InvalidVouchers[0].Voucher.Should().Be(testVoucherC);
            _basket.InvalidVouchers[1].Voucher.Should().Be(testVoucherA);

            _basket.RemoveVoucher(testVoucherB);

            _basket.InvalidVouchers.Should().HaveCount(1);
            _basket.InvalidVouchers[0].Voucher.Should().Be(testVoucherA);
        }
    }
}