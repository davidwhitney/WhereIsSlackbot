﻿using NUnit.Framework;
using WhereIs.Infrastructure;
using WhereIs.Test.Unit.Fakes;

namespace WhereIs.Test.Unit.Infrastructure
{
    [TestFixture]
    public class UrlHelperTests
    {
        [Test]
        public void UrlFor_GivenMethodName_ConstructsKeydUri()
        {
            var helper = new UrlHelper(new FakeConfiguration());

            var url = helper.ForUrl("foo");

            Assert.That(url, Is.EqualTo("https://localhost/api/foo?code=key123"));
        }
    }
}