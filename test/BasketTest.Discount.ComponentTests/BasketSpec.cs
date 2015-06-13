using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts;
using BasketTest.Discounts.Enums;
using BasketTest.Discounts.Items;
using BasketTest.Discounts.VoucherValidation;
using BasketTest.Discounts.VoucherValidation.Gift;
using BasketTest.Discounts.VoucherValidation.Offer;
using FluentAssertions;
using NUnit.Framework;

namespace BasketTest.Discount.ComponentTests
{
    [TestFixture]
    public class BasketSpec
    {
        private IVoucherValidator _giftvalidator;
        private OfferVoucherValidatorAdaptor _offerValidator;
        private Basket _basket;

        [SetUp]
        public void Setup()
        {
            var giftVoucherValueValidator = new GiftVoucherValueValidator();
            var giftVouchersNotInTotalValidator = new GiftVouchersNotInTotalValidator(giftVoucherValueValidator);
            _giftvalidator = new GiftVoucherValidatorAdaptor(giftVouchersNotInTotalValidator);

            var offerSingleValidator = new SingleOfferVoucherValidator();
            var offerVoucherThresholdValidator = new OfferVoucherThresholdValidator(offerSingleValidator);
            _offerValidator = new OfferVoucherValidatorAdaptor(offerVoucherThresholdValidator);

            _basket = new Basket(new List<IVoucherValidator> {_giftvalidator, _offerValidator});
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

        [Test]
        public void Basket_CreatesInvalidVoucher_WhenProductIsRemoved()
        {
            var testProductA = new Product("Hat", 10m);
            var testProductB = new Product("Hat", 10m);
            var testVoucher = new GiftVoucher(15m);

            _basket.AddProduct(testProductA);
            _basket.AddProduct(testProductB);
            _basket.AddVoucher(testVoucher);

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
            _basket.AddVoucher(testVoucher);

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
            _basket.AddVoucher(testVoucherA);
            _basket.AddVoucher(testVoucherB);
            _basket.AddVoucher(testVoucherC);

            _basket.InvalidVouchers.Should().HaveCount(2);
            _basket.InvalidVouchers[0].Voucher.Should().Be(testVoucherC);
            _basket.InvalidVouchers[1].Voucher.Should().Be(testVoucherA);

            _basket.RemoveVoucher(testVoucherB);

            _basket.InvalidVouchers.Should().HaveCount(1);
            _basket.InvalidVouchers[0].Voucher.Should().Be(testVoucherA);
        }

        [Test]
        public void Basket_CaculatesOffer()
        {
            var testProductA = new Product("Hat", 25m);
            var testProductB = new Product("£30 Gift Voucher", 30m, ProductCategory.GiftVoucher);
            var testVoucher = new OfferVoucher(5m, 50m);

            _basket.AddProduct(testProductA);
            _basket.AddProduct(testProductB);
            _basket.AddVoucher(testVoucher);

            _basket.Total().Should().Be(55m);
            _basket.InvalidVouchers.First().Reason.Should().Be(
                "You have not reached the spend threshold. Spend another £25.01 to receive £5.00 discount.");
        }

        [Test]
        public void Basket_CalculatesOfferAndGift()
        {
            var testProductA = new Product("Hat", 25m);
            var testProductB = new Product("Jumper", 26m);
            var testVoucherA = new OfferVoucher(5m, 50m);
            var testVoucherB = new GiftVoucher(5m);

            _basket.AddProduct(testProductA);
            _basket.AddProduct(testProductB);
            _basket.AddVoucher(testVoucherA);
            _basket.AddVoucher(testVoucherB);

            _basket.Total().Should().Be(41m);
        }
    }
}