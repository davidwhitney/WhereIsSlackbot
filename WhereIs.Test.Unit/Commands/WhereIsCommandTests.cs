using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
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
        private ExecutionContext _executionContext;

        [SetUp]
        public void SetUp()
        {
            _logger = new FakeLogger();
            _executionContext = new ExecutionContext {FunctionAppDirectory = Environment.CurrentDirectory};
            _knownLocations = new LocationCollection
            {
                new Location("Foo"),
                new Location("Bar"),
                new Location("Baz")
            };
            
            _sut = new WhereIsCommand(new LocationFinder(new FakeLocationRepository(_knownLocations)),
                new UrlHelper(new Configuration {UrlRoot = "https://localhost/api", ApiKey = "key123"}));
        }

        [Test]
        public async Task Invoke_NoValidDetailsFound_ReturnsFriendlyError()
        {
            var request = ExpectedRequests.WhereIsFor(null);

            var response = await _sut.Run(request, _executionContext, _logger).AsSlackResponse();

            Assert.That(response.text, Is.EqualTo("Sorry! We can't find that place either."));
        }

        [Test]
        public async Task Invoke_KnownLocation_ReturnsLocation()
        {
            var request = ExpectedRequests.WhereIsFor("Foo");

            var response = await _sut.Run(request, _executionContext, _logger).AsSlackResponse();

            Assert.That(response.text, Is.EqualTo("Foo"));
        }

        [Test]
        public async Task Invoke_KnownLocation_ReturnsLocationMap()
        {
            var request = ExpectedRequests.WhereIsFor("Foo");

            var response = await _sut.Run(request, _executionContext, _logger).AsSlackResponse();

            Assert.That(response.attachments[0].image_url, Is.EqualTo("https://localhost/api/Map?code=key123&key=foo"));
        }

        [Test]
        public async Task Invoke_MisspeltLocation_ReturnsLocation()
        {
            var request = ExpectedRequests.WhereIsFor("Fop");

            var response = await _sut.Run(request, _executionContext, _logger).AsSlackResponse();

            Assert.That(response.text, Is.EqualTo("Foo"));
        }
    }
}
