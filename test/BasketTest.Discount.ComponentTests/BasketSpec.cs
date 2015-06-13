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
        private IVoucherValidator _validator;
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
            _basket.AddVoucher(testVoucher);

            _basket.Total().Should().Be(10.50m);
        }

        [Test]
        public void Basket_CreatesInvalidVoucher_ForTooLargeDiscount()
        {
            var testProduct = new Product("Hat", 25m);
            var testVoucher = new GiftVoucher(30m);

            _basket.AddProduct(testProduct);
            _basket.AddVoucher(testVoucher);

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
            _basket.AddVoucher(testVoucher);

            _basket.InvalidVouchers.Should().HaveCount(1);
            var invalidVoucher = _basket.InvalidVouchers.Single();
            invalidVoucher.Voucher.Should().Be(testVoucher);
            invalidVoucher.Reason.Should().Be(
                "Your total must be above the voucher value, not including gift vouchers.");
        }
    }
}