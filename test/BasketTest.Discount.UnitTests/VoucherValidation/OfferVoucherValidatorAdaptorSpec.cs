using System.Collections.Generic;
using BasketTest.Discounts.Items;
using BasketTest.Discounts.VoucherValidation.Offer;
using Moq;
using NUnit.Framework;

namespace BasketTest.Discount.UnitTests.VoucherValidation
{
    [TestFixture]
    public class OfferVoucherValidatorAdaptorSpec
    {
        private Mock<IOfferVoucherValidator> _validatorMock;
        private OfferVoucherValidatorAdaptor _sut;

        [SetUp]
        public void Setup()
        {
            _validatorMock = new Mock<IOfferVoucherValidator>();
            _sut = new OfferVoucherValidatorAdaptor(_validatorMock.Object);
        }

        [Test]
        public void Adaptor_PassesOfferVouchersToValidator()
        {
            var offerVoucherA = new OfferVoucher(10m, 10m);
            var offerVoucherB = new OfferVoucher(10m, 10m);
            var giftVoucher = new GiftVoucher(10m);
            var mixedList = new List<Voucher>
            {
                offerVoucherA, offerVoucherB, giftVoucher
            };

            _sut.Validate(new List<Product>(), mixedList);

            _validatorMock.Verify(v => v.Validate(
                It.IsAny<List<Product>>(), It.Is<List<OfferVoucher>>(list => list.Count == 2)));
        }
    }
}
