﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
            _sut.Products.Single().Should.Be(testProduct);
        }
    }
}