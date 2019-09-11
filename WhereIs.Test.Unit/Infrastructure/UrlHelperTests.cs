using NUnit.Framework;
using WhereIs.Infrastructure;

namespace WhereIs.Test.Unit.Infrastructure
{
    [TestFixture]
    public class UrlHelperTests
    {
        [Test]
        public void UrlFor_GivenMethodName_ConstructsKeydUri()
        {
            var helper = new UrlHelper(new Configuration{UrlRoot = "https://localhost/api", ApiKey = "key123"});

            var url = helper.ForUrl("foo");

            Assert.That(url, Is.EqualTo("https://localhost/api/foo?code=key123"));
        }
    }
}
