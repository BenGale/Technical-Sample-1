using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Items;
using BasketTest.Discounts.VoucherValidation.Offer;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BasketTest.Discount.UnitTests.VoucherValidation.Offer
{
    [TestFixture]
    public class OfferVoucherThresholdValidatorSpec
    {
        private Mock<IOfferVoucherValidator> _offerValidatorMock;
        private OfferVoucherThresholdValidator _sut;

        [SetUp]
        public void Setup()
        {
            _offerValidatorMock = new Mock<IOfferVoucherValidator>();
            _offerValidatorMock.Setup(v => v.Validate(
                It.IsAny<List<Product>>(), It.IsAny<List<OfferVoucher>>()))
                .Returns(new List<InvalidVoucher>());
            _sut = new OfferVoucherThresholdValidator(_offerValidatorMock.Object);
        }

        [Test]
        public void Validator_RemovesVoucherOverThreshold()
        {
            var testVoucher = new OfferVoucher(10m, 20m);
            var testProducts = new List<Product>
            {
                new Product("Hat", 15m),
            };

            var result = _sut.Validate(testProducts, new List<OfferVoucher> {testVoucher});

            result.Should().HaveCount(1);
            result.First().Voucher.Should().Be(testVoucher);
        }
    }
}
