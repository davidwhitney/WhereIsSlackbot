using System;
using System.Threading.Tasks;
using NUnit.Framework;
using WhereIs.FindingPlaces;
using WhereIs.Infrastructure;
using WhereIs.Test.Unit.Fakes;

namespace WhereIs.Test.Unit
{
    [TestFixture]
    public class CapacityCommandTests
    {
        private CapacityCommand _sut;
        private FakeLogger _logger;
        private LocationCollection _knownLocations;

        [SetUp]
        public void SetUp()
        {
            _logger = new FakeLogger();
            _knownLocations = new LocationCollection
            {
                new Location("Foo"),
                new Location("Bar"),
                new Location("Baz")
            };

            _sut = new CapacityCommand(new LocationFinder(_knownLocations),
                new UrlHelper(new Configuration { UrlRoot = "https://localhost/api", ApiKey = "key123" }));
        }

        [Test]
        [Ignore("blah")]
        public async Task Run_NoLocationRequested_ReturnsFriendlyError()
        {
            var request = ExpectedRequests.WhereIsFor(null);

            var response = await _sut.Execute(request, _logger).AsSlackResponse();

            Assert.That(response.text, Is.EqualTo("Sorry! You need to specify a location."));
        }

        [Test]
        [Ignore("blah2")]
        public async Task Run_KnownLocationRequested_ReturnsHintAsToAvailability()
        {
            var request = ExpectedRequests.WhereIsFor("Foo");

            var response = await _sut.Execute(request, _logger).AsSlackResponse();

            Assert.That(response.text, Is.EqualTo("Foo"));
        }
    }
}
