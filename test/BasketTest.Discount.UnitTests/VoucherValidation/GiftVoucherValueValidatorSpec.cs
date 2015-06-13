using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Items;
using BasketTest.Discounts.VoucherValidation;
using FluentAssertions;
using NUnit.Framework;

namespace BasketTest.Discount.UnitTests.VoucherValidation
{
    [TestFixture]
    public class GiftVoucherValueValidatorSpec
    {
        private GiftVoucherValueValidator _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new GiftVoucherValueValidator();
        }

        [Test]
        public void Validator_ReturnsEmptyWhenValid()
        {
            var testProduct = new Product("Hat", 10m);
            var testVoucher = new GiftVoucher(5m);

            var result = _sut.Validate(
                new List<Product> {testProduct}, new List<GiftVoucher> {testVoucher});

            result.Should().HaveCount(0);
        }

        [Test]
        public void Validator_ReturnsInvalid_WithLargeVoucher()
        {
            var testProduct = new Product("Hat", 10m);
            var testVoucher = new GiftVoucher(15m);

            var result = _sut.Validate(
                new List<Product> { testProduct }, new List<GiftVoucher> { testVoucher });

            result.Should().HaveCount(1);
            var invalidVoucher = result.First();
            invalidVoucher.Voucher.Should().Be(testVoucher);
            invalidVoucher.Reason.Should().Be(
                "Your total must be above the voucher value, not including gift vouchers.");
        }

        [Test]
        public void Validator_ReturnsInvalid_WithMultiples()
        {
            var testProduct = new Product("Hat", 7.50m);
            var testVoucherA = new GiftVoucher(5m);
            var testVoucherB = new GiftVoucher(5m);

            var result = _sut.Validate(
                new List<Product> { testProduct }, 
                new List<GiftVoucher> { testVoucherA, testVoucherB });

            result.Should().HaveCount(1);
            var invalidVoucher = result.First();
            invalidVoucher.Voucher.Should().Be(testVoucherB);
            invalidVoucher.Reason.Should().Be(
                "Your total must be above the voucher value, not including gift vouchers.");
        }

        [Test]
        public void Validator_ReturnsSmallestInvalid_WithMultiples()
        {
            var testProduct = new Product("Hat", 7.50m);
            var testVoucherA = new GiftVoucher(5m);
            var testVoucherB = new GiftVoucher(7m);

            var result = _sut.Validate(
                new List<Product> { testProduct },
                new List<GiftVoucher> { testVoucherA, testVoucherB });

            result.Should().HaveCount(1);
            var invalidVoucher = result.First();
            invalidVoucher.Voucher.Should().Be(testVoucherA);
            invalidVoucher.Reason.Should().Be(
                "Your total must be above the voucher value, not including gift vouchers.");
        }
    }
}