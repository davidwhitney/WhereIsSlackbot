using System;
using System.Threading.Tasks;
using NUnit.Framework;
using WhereIs.Commands;
using WhereIs.FindingPlaces;
using WhereIs.Infrastructure;
using WhereIs.Test.Unit.Fakes;

namespace WhereIs.Test.Unit.Commands
{
    [TestFixture]
    public class WhereIsCommandTests
    {
        private WhereIsCommand _sut;
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
            
            _sut = new WhereIsCommand(new LocationFinder(_knownLocations),
                new UrlHelper(new Configuration {UrlRoot = "https://localhost/api", ApiKey = "key123"}));
        }

        [Test]
        public async Task Run_NoValidDetailsFound_ReturnsFriendlyError()
        {
            var request = ExpectedRequests.WhereIsFor(null);

            var response = await _sut.Execute(request, _logger).AsSlackResponse();

            Assert.That(response.text, Is.EqualTo("Sorry! We can't find that place either."));
        }

        [Test]
        public async Task Run_KnownLocation_ReturnsLocation()
        {
            var request = ExpectedRequests.WhereIsFor("Foo");

            var response = await _sut.Execute(request, _logger).AsSlackResponse();

            Assert.That(response.text, Is.EqualTo("Foo"));
        }

        [Test]
        public async Task Run_KnownLocation_ReturnsLocationMap()
        {
            var request = ExpectedRequests.WhereIsFor("Foo");

            var response = await _sut.Execute(request, _logger).AsSlackResponse();

            Assert.That(response.attachments[0].image_url, Is.EqualTo("https://localhost/api/Map?code=key123&key=foo"));
        }

        [Test]
        public async Task Run_KnownLocation_LocationMapHasCaption()
        {
            var request = ExpectedRequests.WhereIsFor("Foo");

            var response = await _sut.Execute(request, _logger).AsSlackResponse();

            Assert.That(response.attachments[0].text, Is.EqualTo("Foo is marked on the map."));
        }

        [Test]
        public async Task Run_MisspeltLocation_ReturnsLocation()
        {
            var request = ExpectedRequests.WhereIsFor("Fop");

            var response = await _sut.Execute(request, _logger).AsSlackResponse();

            Assert.That(response.text, Is.EqualTo("Foo"));
        }

        [Test]
        public async Task Run_ErrorIsThrown_LogsAndRethrows()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _sut.Execute(null, _logger));

            Assert.That(_logger.Entries.Count, Is.EqualTo(1));
        }
    }
}
