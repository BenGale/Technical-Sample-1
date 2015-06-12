using System;
using BasketTest.Discounts.Items;
using FluentAssertions;
using NUnit.Framework;

namespace BasketTest.Discount.UnitTests.Items
{
    [TestFixture]
    public class GiftVoucherSpec
    {
        [Test]
        public void GiftVoucher_AcceptsNegativeValue()
        {
            Action act = () =>
            {
                var sut = new GiftVoucher(-10m);
            };

            act.ShouldNotThrow();
        }

        [Test]
        public void GiftVoucher_CantHavePositiveValue()
        {
            Action act = () =>
            {
                var sut = new GiftVoucher(10m);
            };

            act.ShouldThrow<ArgumentOutOfRangeException>()
                .WithMessage("Value cannot be positive.");
        }
    }
}