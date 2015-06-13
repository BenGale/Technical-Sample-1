using System;
using BasketTest.Discounts.Items;
using FluentAssertions;
using NUnit.Framework;

namespace BasketTest.Discount.UnitTests.Items
{
    [TestFixture]
    public class OfferVoucherSpec
    {
        [Test]
        public void GiftVoucher_RejectsNegativeValue()
        {
            Action act = () =>
            {
                var sut = new OfferVoucher(-10m, 10m);
            };
            act.ShouldThrow<ArgumentException>()
                .WithMessage("Value cannot be negative.");
            
        }

        [Test]
        public void GiftVoucher_AcceptsPositiveValue()
        {
            Action act = () =>
            {
                var sut = new OfferVoucher(10m, 10m);
            };
            act.ShouldNotThrow();
        }

        [Test]
        public void GiftVoucher_RejectsNegativeThreshold()
        {
            Action act = () =>
            {
                var sut = new OfferVoucher(10m, -10m);
            };
            act.ShouldThrow<ArgumentException>()
                .WithMessage("Value cannot be negative.");

        }
    }
}