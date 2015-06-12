﻿using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Enums;
using BasketTest.Discounts.Items;
using BasketTest.Discounts.VoucherValidation;
using FluentAssertions;
using NUnit.Framework;

namespace BasketTest.Discount.UnitTests.VoucherValidation
{
    [TestFixture]
    public class GiftVouchersNotInTotalValidatorSpec
    {
        private GiftVouchersNotInTotalValidator _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new GiftVouchersNotInTotalValidator();
        }

        [Test]
        public void Validator_ReturnsEmptyWhenValid()
        {
            var testProduct = new Product("Hat", 10m);
            var testVoucher = new GiftVoucher(5m);

            var result = _sut.Validate(
                new List<Product> { testProduct }, new List<GiftVoucher> { testVoucher });

            result.Should().HaveCount(0);
        }

        [Test]
        public void Validator_ReturnsInvalid_WhenProductIsVoucher()
        {
            var testProduct = new Product("Gift Voucher", 10m, ProductCategory.GiftVoucher);
            var testVoucher = new GiftVoucher(15m);

            var result = _sut.Validate(
                new List<Product> { testProduct }, new List<GiftVoucher> { testVoucher });

            result.Should().HaveCount(1);
            var invalidVoucher = result.First();
            invalidVoucher.Voucher.Should().Be(testVoucher);
            invalidVoucher.Reason.Should().Be(
                "Your total must be above the voicher value, not including gift vouchers.");
        }
    }
}
