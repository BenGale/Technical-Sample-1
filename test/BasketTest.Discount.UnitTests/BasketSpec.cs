using System.Collections.Generic;
using System.Linq;
using BasketTest.Discounts;
using BasketTest.Discounts.Items;
using BasketTest.Discounts.VoucherValidation;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BasketTest.Discount.UnitTests
{
    [TestFixture]
    public class BasketSpec
    {
        private Basket _sut;
        private Mock<IVoucherValidator> _validatorMock;

        [SetUp]
        public void Setup()
        {
            _validatorMock = new Mock<IVoucherValidator>();
            _validatorMock.Setup(validator =>
                validator.Validate(It.IsAny<List<Product>>(), It.IsAny<List<GiftVoucher>>()))
                .Returns(new List<InvalidVoucher>());
            _sut = new Basket(_validatorMock.Object);
        }

        [Test]
        public void Basket_AcceptsProduct()
        {
            var testProduct = new Product("Hat", 10.50m);

            _sut.AddProduct(testProduct);

            _sut.Products.Should().HaveCount(1);
            _sut.Products.Single().Should().Be(testProduct);
        }

        [Test]
        public void Basket_AcceptsMultipleProducts()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);

            _sut.Products.Should().HaveCount(2);
            _sut.Products.Should().Contain(testProductA);
            _sut.Products.Should().Contain(testProductB);
        }

        [Test]
        public void Basket_CalculatesProductsTotal()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);
            decimal result =_sut.Total();

            result.Should().Be(65.15m);
        }

        [Test]
        public void Basket_AcceptsGiftVoucher()
        {
            var testProduct = new Product("Hat", 10m);
            var testVoucher = new GiftVoucher(5m);

            _sut.AddProduct(testProduct);
            _sut.AddVoucher(testVoucher);

            _sut.Vouchers.Should().HaveCount(1);
            _sut.Vouchers.Single().Should().Be(testVoucher);
        }

        [Test]
        public void Basket_AcceptsMultipleGiftVouchers()
        {
            var testProduct = new Product("Hat", 30m);
            var testVoucherA = new GiftVoucher(5m);
            var testVoucherB = new GiftVoucher(15m);

            _sut.AddProduct(testProduct);
            _sut.AddVoucher(testVoucherA);
            _sut.AddVoucher(testVoucherB);

            _sut.Vouchers.Should().HaveCount(2);
            _sut.Vouchers.Should().Contain(testVoucherA);
            _sut.Vouchers.Should().Contain(testVoucherB);
        }

        [Test]
        public void Basket_CalulatesWithGiftVouchers()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);
            var testVoucher = new GiftVoucher(5m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);
            _sut.AddVoucher(testVoucher);

            _sut.Total().Should().Be(60.15m);
        }

        [Test]
        public void Basket_CalulatesWithMulitpleGiftVouchers()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);
            var testVoucherA = new GiftVoucher(5m);
            var testVoucherB = new GiftVoucher(10m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);
            _sut.AddVoucher(testVoucherA);
            _sut.AddVoucher(testVoucherB);

            _sut.Total().Should().Be(50.15m);
        }

        [Test]
        public void Basket_AcceptsProductRemoval()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);

            _sut.Products.Should().HaveCount(2);

            _sut.RemoveProduct(testProductA);

            _sut.Products.Should().HaveCount(1);
            _sut.Products.Should().NotContain(testProductA);
            _sut.Products.Should().Contain(testProductB);
        }

        [Test]
        public void Basket_AcceptsVoucherRemoval()
        {
            var testProduct = new Product("Hat", 30m);
            var testVoucherA = new GiftVoucher(5m);
            var testVoucherB = new GiftVoucher(15m);

            _sut.AddProduct(testProduct);
            _sut.AddVoucher(testVoucherA);
            _sut.AddVoucher(testVoucherB);

            _sut.Vouchers.Should().HaveCount(2);

            _sut.RemoveVoucher(testVoucherA);

            _sut.Vouchers.Should().HaveCount(1);
            _sut.Vouchers.Should().NotContain(testVoucherA);
            _sut.Vouchers.Should().Contain(testVoucherB);
        }

        [Test]
        public void Basket_CaluclatesProductsTotal_AfterRemoval()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);
            _sut.RemoveProduct(testProductA);
            decimal result = _sut.Total();

            result.Should().Be(54.65m);
        }

        [Test]
        public void Basket_CaluclatesTotalWithVouchers_AfterRemoval()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);
            var testVoucherA = new GiftVoucher(5m);
            var testVoucherB = new GiftVoucher(10m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);
            _sut.AddVoucher(testVoucherA);
            _sut.AddVoucher(testVoucherB);
            _sut.RemoveVoucher(testVoucherA);

            _sut.Total().Should().Be(55.15m);
        }

        [Test]
        public void Basket_Validates_OnProductAdded()
        {
            var testInvalidVoucher = new InvalidVoucher(
                new GiftVoucher(1m), "Test Reason");
            _validatorMock.Setup(validator =>
                validator.Validate(It.IsAny<List<Product>>(), It.IsAny<List<GiftVoucher>>()))
                .Returns(new List<InvalidVoucher> { testInvalidVoucher });

            _sut.AddProduct(new Product("Hat", 10m));

            _validatorMock.Verify(validator =>
                validator.Validate(It.IsAny<List<Product>>(), It.IsAny<List<GiftVoucher>>()), 
                Times.Once);
            _sut.InvalidVouchers.Should().HaveCount(1);
            _sut.InvalidVouchers.Single().Should().Be(testInvalidVoucher);
        }

        [Test]
        public void Basket_Validates_OnProductRemoved()
        {
            var testInvalidVoucher = new InvalidVoucher(
                new GiftVoucher(1m), "Test Reason");
            _validatorMock.Setup(validator =>
                validator.Validate(It.IsAny<List<Product>>(), It.IsAny<List<GiftVoucher>>()))
                .Returns(new List<InvalidVoucher> { testInvalidVoucher });

            _sut.AddProduct(new Product("Hat", 10m));
            _sut.RemoveProduct(new Product("Hat", 10m));

            _validatorMock.Verify(validator =>
                validator.Validate(It.IsAny<List<Product>>(), It.IsAny<List<GiftVoucher>>()),
                Times.Exactly(2));
            _sut.InvalidVouchers.Should().HaveCount(1);
            _sut.InvalidVouchers.Single().Should().Be(testInvalidVoucher);
        }

        [Test]
        public void Basket_Validates_OnVoucherAdded()
        {
            var testVoucher = new GiftVoucher(1m);
            var testInvalidVoucher = new InvalidVoucher(testVoucher, "Test Reason");
            _validatorMock.Setup(validator =>
                validator.Validate(It.IsAny<List<Product>>(), It.IsAny<List<GiftVoucher>>()))
                .Returns(new List<InvalidVoucher> { testInvalidVoucher });

            _sut.AddVoucher(testVoucher);

            _validatorMock.Verify(validator =>
                validator.Validate(It.IsAny<List<Product>>(), It.IsAny<List<GiftVoucher>>()),
                Times.Once);
            _sut.InvalidVouchers.Should().HaveCount(1);
            _sut.InvalidVouchers.Single().Should().Be(testInvalidVoucher);
        }

        [Test]
        public void Basket_Validates_OnVoucherRemoved()
        {
            var testVoucher = new GiftVoucher(1m);
            var testInvalidVoucher = new InvalidVoucher(testVoucher, "Test Reason");
            _validatorMock.Setup(validator =>
                validator.Validate(It.IsAny<List<Product>>(), It.IsAny<List<GiftVoucher>>()))
                .Returns(new List<InvalidVoucher> { testInvalidVoucher });

            _sut.AddVoucher(testVoucher);
            _sut.RemoveVoucher(testVoucher);

            _validatorMock.Verify(validator =>
                validator.Validate(It.IsAny<List<Product>>(), It.IsAny<List<GiftVoucher>>()),
                Times.Exactly(2));
            _sut.InvalidVouchers.Should().HaveCount(1);
            _sut.InvalidVouchers.Single().Should().Be(testInvalidVoucher);
        }

        [Test]
        public void Basket_AllowsRemovalOfInvalidVoucher()
        {
            var testVoucher = new GiftVoucher(1m);
            _validatorMock.Setup(validator =>
                validator.Validate(It.IsAny<List<Product>>(), It.IsAny<List<GiftVoucher>>()))
                .Returns((List<Product> products, List<GiftVoucher> vouchers) =>
                {
                    return vouchers.Select(voucher => new InvalidVoucher(voucher, "Test")).ToList();
                });

            _sut.AddVoucher(testVoucher);

            _sut.InvalidVouchers.Should().HaveCount(1);
            _sut.InvalidVouchers.Single().Voucher.Should().Be(testVoucher);

            _sut.RemoveVoucher(testVoucher);
            _sut.Vouchers.Should().HaveCount(0);
            _sut.InvalidVouchers.Should().HaveCount(0);
        }

        [Test]
        public void Basket_AcceptsOfferVoucher()
        {
            var testOffer = new OfferVoucher(10m, 15m);

            _sut.AddOfferVoucher(testOffer);

            _sut.OfferVouchers.Should().HaveCount(1);
        }
    }
}