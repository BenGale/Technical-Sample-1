using System.Collections.Generic;
using BasketTest.Discounts.Items;
using BasketTest.Discounts.VoucherValidation.Offer;
using FluentAssertions;
using NUnit.Framework;

namespace BasketTest.Discount.UnitTests.VoucherValidation.Offer
{
    [TestFixture]
    public class SingleOfferVoucherValidatorSpec
    {
        private SingleOfferVoucherValidator _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new SingleOfferVoucherValidator();
        }

        [Test]
        public void Validator_OnlyAllowsSingleOffer()
        {
            var testVoucherA = new OfferVoucher(10m, 10m);
            var testVoucherB = new OfferVoucher(10m, 10m);
            var testVoucherC = new OfferVoucher(10m, 10m);

            var result = _sut.Validate(new List<Product>(), new List<OfferVoucher>
            {
                testVoucherA,
                testVoucherB,
                testVoucherC
            });

            result.Should().HaveCount(2);
        }

        [Test]
        public void Validator_OnlyAllowsSingleOffer_UsesHighest()
        {
            var testVoucherA = new OfferVoucher(10m, 10m);
            var testVoucherB = new OfferVoucher(12m, 10m);
            var testVoucherC = new OfferVoucher(15m, 10m);

            var result = _sut.Validate(new List<Product>(), new List<OfferVoucher>
            {
                testVoucherA,
                testVoucherB,
                testVoucherC
            });

            result.Should().HaveCount(2);
            result[0].Voucher.Should().Be(testVoucherA);
            result[1].Voucher.Should().Be(testVoucherB);
        }
    }
}
