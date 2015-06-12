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
        public void GiftVoucher_RejectsNegativeValue()
        {
            Action act = () =>
            {
                var sut = new GiftVoucher(-10m);
            };
            act.ShouldThrow<ArgumentException>()
                .WithMessage("Value cannot be negative.");
            
        }

        [Test]
        public void GiftVoucher_AcceptsPositiveValue()
        {
            Action act = () =>
            {
                var sut = new GiftVoucher(10m);
            };
            act.ShouldNotThrow();
        }
    }
}