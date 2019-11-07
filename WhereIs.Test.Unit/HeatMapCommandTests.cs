using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using WhereIs.FindingPlaces;
using WhereIs.ImageGeneration;
using WhereIs.Infrastructure;
using WhereIs.Test.Unit.Fakes;

namespace WhereIs.Test.Unit
{
    [TestFixture]
    public class HeatMapCommandTests
    {
        private HeatMapCommand _sut;
        private FakeLogger _logger;
        private LocationCollection _knownLocations;
        private MemoryCache _cache;
        private FakeGenerator _fakeGenerator;
        private FakeCapacityService _capacityService;

        [SetUp]
        public void SetUp()
        {
            _logger = new FakeLogger();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _fakeGenerator = new FakeGenerator {Returns = new byte[0]};
            _knownLocations = new LocationCollection
            {
                new Location("Foo"),
                new Location("Bronte", new ImageLocation(0, 0, "gracechurch")),
                new Location("gracechurch::245-210", new ImageLocation(245, 210, "gracechurch"))
                {
                    Capacity = 100
                },
            };

            _capacityService = new FakeCapacityService();

            _sut = new HeatMapCommand(_knownLocations, _fakeGenerator, _cache, _capacityService);
        }
        
        [Test]
        public void Execute_ForKnownKey_ReturnsJpegContentType()
        {
            var request = ExpectedRequests.MapRequestForKey("gracechurch");

            var response = _sut.Execute(request, _logger).AsFile();

            Assert.That(response.ContentType, Is.EqualTo("image/jpeg"));
        }

        [Test]
        public void Execute_ForKnownKey_ReturnsJpegFileFromImageGenerator()
        {
            _fakeGenerator.Returns = new byte[] {1, 2, 3, 4};
            var request = ExpectedRequests.MapRequestForKey("gracechurch");

            var response = _sut.Execute(request, _logger).AsFile();

            Assert.That(response.FileContents, Is.EqualTo(_fakeGenerator.Returns));
        }

        [Test]
        public void Blah_realthing()
        {
            _sut = new HeatMapCommand(_knownLocations, new ImageGenerator(new Configuration()), _cache, _capacityService);
            var request = ExpectedRequests.MapRequestForKey("gracechurch");

            var response = _sut.Execute(request, _logger).AsFile();

            Assert.That(response.FileContents, Is.Not.Null);
        }
        
        [Test]
        public void Execute_ErrorIsThrown_LogsAndRethrows()
        {
            Assert.Throws<NullReferenceException>(() => _sut.Execute(null, _logger));

            Assert.That(_logger.Entries.Count, Is.EqualTo(1));
        }
    }
}