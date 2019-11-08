using System;
using System.Linq;
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
        private FakeCapacityService _capacityService;

        [SetUp]
        public void SetUp()
        {
            _logger = new FakeLogger();
            _knownLocations = new LocationCollection
            {
                new Location("Foo"),
                new Location("Bronte", new ImageLocation(0, 0, "gracechurch")),
                new Location("gracechurch::245-210", new ImageLocation(245, 210, "gracechurch")),
            };

            _capacityService = new FakeCapacityService();

            _sut = new CapacityCommand(_knownLocations,
                new UrlHelper(new Configuration { UrlRoot = "https://localhost/api", ApiKey = "key123" }),
                _capacityService);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task Run_NoLocationRequested_ReturnsFriendlyError(string messageContents)
        {
            var request = ExpectedRequests.CapacityFor(messageContents);

            var response = await _sut.Execute(request, _logger).AsSlackResponse();

            Assert.That(response.text, Is.EqualTo("Sorry! You need to specify a location."));
        }

        [TestCase("Gracechurch")]
        [TestCase("gracechurch")]
        public async Task Run_KnownLocationRequested_ReturnsHintAsToAvailability(string locName)
        {
            var request = ExpectedRequests.CapacityFor(locName);
            var capacity = new Random().Next(0, 100);
            var used = new Random().Next(0, 100);
            _knownLocations.Last().Capacity = capacity;
            _capacityService.ReturnsThis = used;
            
            var response = await _sut.Execute(request, _logger).AsSlackResponse();

            Assert.That(response.text, Is.EqualTo($"There are {used} of {capacity} desks used in {locName}."));
            Assert.That(response.attachments.Single().image_url, Is.Not.Null);
        }        
        
        [Test]
        public async Task Run_KnownLocationRequested_IncludesAHeatMapImage()
        {
            var request = ExpectedRequests.CapacityFor("gracechurch");
            
            var response = await _sut.Execute(request, _logger).AsSlackResponse();

            Assert.That(response.attachments.Single().image_url, Is.Not.Null);
            Assert.That(response.attachments.Single().image_url, Does.Contain("HeatMap"));
        }

        [Test]
        public void Execute_ErrorIsThrown_LogsAndRethrows()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _sut.Execute(null, _logger));

            Assert.That(_logger.Entries.Count, Is.EqualTo(1));
        }
    }
}
