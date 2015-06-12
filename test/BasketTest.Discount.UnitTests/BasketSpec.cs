using System.Linq;
using BasketTest.Discounts;
using FluentAssertions;
using NUnit.Framework;

namespace BasketTest.Discount.UnitTests
{
    [TestFixture]
    public class BasketSpec
    {
        private Basket _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new Basket();
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
        public void Basket_CalculatesTotal()
        {
            var testProductA = new Product("Hat", 10.50m);
            var testProductB = new Product("Jumper", 54.65m);

            _sut.AddProduct(testProductA);
            _sut.AddProduct(testProductB);
            decimal result =_sut.Total();

            result.Should().Be(65.15m);
        }
    }
}
