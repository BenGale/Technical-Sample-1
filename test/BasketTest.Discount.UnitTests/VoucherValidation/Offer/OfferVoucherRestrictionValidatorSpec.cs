﻿using System;
using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts.Enums;
using BasketTest.Discounts.Items;
using BasketTest.Discounts.VoucherValidation.Offer;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BasketTest.Discount.UnitTests.VoucherValidation.Offer
{
    [TestFixture]
    public class OfferVoucherRestrictionValidatorSpec
    {
        private Mock<IOfferVoucherValidator> _offerValidatorMock;
        private OfferVoucherRestrictionValidator _sut;

        [SetUp]
        public void Setup()
        {
            _offerValidatorMock = new Mock<IOfferVoucherValidator>();
            _offerValidatorMock.Setup(v => v.Validate(
                It.IsAny<List<Product>>(), It.IsAny<List<OfferVoucher>>()))
                .Returns(new List<InvalidVoucher>());
            _sut = new OfferVoucherRestrictionValidator(_offerValidatorMock.Object);
        }

        [Test]
        public void Validator_RemovesVoucher_WithNoMatchingCategory()
        {
            var testVoucher = new OfferVoucher(10m, 50m, ProductCategory.HeadWear);
            var testProduct = new Product("Laptop", 1000m);

            var result = _sut.Validate(new List<Product> {testProduct}, new List<OfferVoucher> {testVoucher});

            result.Should().HaveCount(1);
            result.First().Voucher.Should().Be(testVoucher);
            result.First().Reason.Should().Be("There are no products in your basket applicable to this voucher.");
        }
    }
}
