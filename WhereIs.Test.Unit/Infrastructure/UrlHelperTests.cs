using NUnit.Framework;
using WhereIs.Infrastructure;

namespace WhereIs.Test.Unit.Infrastructure
{
    [TestFixture]
    public class UrlHelperTests
    {
        private UrlHelper _sut;
        [SetUp]
        public void SetUp() => _sut = new UrlHelper(new Configuration { UrlRoot = "https://localhost/api", ApiKey = "key123" });

        [TestCase("foo", "https://localhost/api/Map?code=key123&key=foo")]
        [TestCase("foo bar", "https://localhost/api/Map?code=key123&key=foo+bar")]
        public void ImageFor_GivenName_ConstructsKeydUri(string key, string expectedUri)
        {
            var url = _sut.ImageFor(key);

            Assert.That(url, Is.EqualTo(expectedUri));
        }
    }
}
