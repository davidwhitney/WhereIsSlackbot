using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;

namespace WhereIs.Test.Unit
{
    [TestFixture]
    public class UrlHelperTests
    {
        [Test]
        public void UrlFor_GivenMethodName_ConstructsKeydUri()
        {
            var helper = new UrlHelper(
                new FakeConfiguration
                {
                    {"Values:ApiKey", "key123"},
                    {"Values:UrlRoot", "https://localhost/api"}
                }
            );

            var url = helper.ForUrl("foo");

            Assert.That(url, Is.EqualTo("https://localhost/api/foo?code=key123"));
        }
    }

    public class FakeConfiguration : Dictionary<string, string>, IConfiguration
    {
        public IConfigurationSection GetSection(string key) => throw new NotImplementedException();

        public IEnumerable<IConfigurationSection> GetChildren() => throw new NotImplementedException();

        public IChangeToken GetReloadToken() => throw new NotImplementedException();
    }
}
