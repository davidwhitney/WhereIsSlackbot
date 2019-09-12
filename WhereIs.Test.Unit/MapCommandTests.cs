using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using WhereIs.FindingPlaces;
using WhereIs.Infrastructure;
using WhereIs.Test.Unit.Fakes;

namespace WhereIs.Test.Unit
{
    [TestFixture]
    public class MapCommandTests
    {
        private MapCommand _sut;
        private FakeLogger _logger;
        private LocationCollection _knownLocations;
        private Configuration _config;
        private MemoryCache _cache;
        private FakeGenerator _fakeGenerator;

        [SetUp]
        public void SetUp()
        {
            _logger = new FakeLogger();
            _config = new Configuration {ApiKey = "key", UrlRoot = "http://something" };
            _cache = new MemoryCache(new MemoryCacheOptions());
            _fakeGenerator = new FakeGenerator {Returns = new byte[0]};
            _knownLocations = new LocationCollection
            {
                new Location("Foo", new ImageLocation(10, 10)),
                new Location("Foo Bar"),
            };
            _sut = new MapCommand(_knownLocations, _fakeGenerator, _cache);
        }

        [Test]
        public void Ctor_DoesNotAcceptNullParams()
        {
            Assert.Throws<ArgumentNullException>(() => new MapCommand(null, _fakeGenerator, _cache));
            Assert.Throws<ArgumentNullException>(() => new MapCommand(_knownLocations, null, _cache));
            Assert.Throws<ArgumentNullException>(() => new MapCommand(_knownLocations, _fakeGenerator, null));
        }

        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("invalid-key")]
        public void Execute_NoValidKeyProvided_Returns404(string key)
        {
            var request = ExpectedRequests.MapRequestForKey(key);

            var response = _sut.Execute(request, _logger);

            Assert.That(response, Is.InstanceOf<NotFoundResult>());
        }

        [TestCase("foo")]
        [TestCase("foo+bar")]
        public void Execute_ForKnownKey_ReturnsJpegContentType(string key)
        {
            var request = ExpectedRequests.MapRequestForKey(key);

            var response = _sut.Execute(request, _logger).AsFile();

            Assert.That(response.ContentType, Is.EqualTo("image/jpeg"));
        }

        [Test]
        public void Execute_ForKnownKey_ReturnsJpegFileFromImageGenerator()
        {
            _fakeGenerator.Returns = new byte[] {1, 2, 3, 4};
            var request = ExpectedRequests.MapRequestForKey(_knownLocations.First().Key);

            var response = _sut.Execute(request, _logger).AsFile();

            Assert.That(response.FileContents, Is.EqualTo(_fakeGenerator.Returns));
        }
        
        [Test]
        public void Execute_ErrorIsThrown_LogsAndRethrows()
        {
            Assert.Throws<NullReferenceException>(() => _sut.Execute(null, _logger));

            Assert.That(_logger.Entries.Count, Is.EqualTo(1));
        }

        [Test]
        public void Execute_ItemsAddedToCache()
        {
            var request = ExpectedRequests.MapRequestForKey(_knownLocations.First().Key);

            _sut.Execute(request, _logger).AsFile();

            Assert.That(_cache.Count, Is.EqualTo(1));
        }

        [Test]
        public void Execute_MultipleRequestsForSameKey_ImageSourcedFromCache()
        {
            var request = ExpectedRequests.MapRequestForKey(_knownLocations.First().Key);

            _sut.Execute(request, _logger).AsFile();
            _sut.Execute(request, _logger).AsFile();

            Assert.That(_fakeGenerator.Called, Is.EqualTo(1));
        }
    }
}