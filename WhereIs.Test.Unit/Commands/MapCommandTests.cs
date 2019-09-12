using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WhereIs.Commands;
using WhereIs.FindingPlaces;
using WhereIs.Infrastructure;
using WhereIs.Test.Unit.Fakes;

namespace WhereIs.Test.Unit.Commands
{
    [TestFixture]
    public class MapCommandTests
    {
        private MapCommand _sut;
        private FakeLogger _logger;
        private LocationCollection _knownLocations;
        private Configuration _config;

        [SetUp]
        public void SetUp()
        {
            _logger = new FakeLogger();
            _config = new Configuration {ApiKey = "key", UrlRoot = "http://something" };
            _knownLocations = new LocationCollection
            {
                new Location("Foo", new ImageLocation(10, 10)),
                new Location("Foo Bar"),
            };
            _sut = new MapCommand(_knownLocations, _config);
        }

        [TestCase(null)]
        [TestCase("invalid-key")]
        public void Execute_NoValidKeyProvided_Returns404(string key)
        {
            var request = ExpectedRequests.MapRequestForKey(key);

            var response = _sut.Execute(request, _logger);

            Assert.That(response, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void Execute_ForKnownKey_ReturnsJpeg()
        {
            var request = ExpectedRequests.MapRequestForKey("foo");

            var response = _sut.Execute(request, _logger).AsFile();

            Assert.That(response.ContentType, Is.EqualTo("image/jpeg"));
        }

        [Test]
        public void Execute_ForKnownKeyWithSpaceInIt_ReturnsJpeg()
        {
            var request = ExpectedRequests.MapRequestForKey(_knownLocations.Last().Key);

            var response = _sut.Execute(request, _logger).AsFile();

            Assert.That(response.ContentType, Is.EqualTo("image/jpeg"));
        }

        [Test]
        public void Execute_ForKnownKey_ReturnsModifiedImage()
        {
            var path = Path.Combine(_config.MapPath, "map.png");
            var defaultMap = File.ReadAllBytes(path);
            var request = ExpectedRequests.MapRequestForKey("foo");

            var response = _sut.Execute(request, _logger).AsFile();

            Assert.That(defaultMap.SequenceEqual(response.FileContents), Is.False);
        }

        [Test]
        public void Execute_ForKnownKeyPlaces_MarkerCoveringLocationOnMapInRed()
        {
            var request = ExpectedRequests.MapRequestForKey("foo");

            var response = _sut.Execute(request, _logger).AsFile();

            var returnedImage = new Bitmap(new MemoryStream(response.FileContents));
            var pixel = returnedImage.GetPixel(10, 10);
            Assert.That(pixel.Name, Is.EqualTo("fffe0000"));
        }

        [Test]
        public void Execute_ForKnownKeyPlaces_MarkerCoveringLocationOnMapInRedIsBiggerThanOnePixel()
        {
            var request = ExpectedRequests.MapRequestForKey("foo");

            var response = _sut.Execute(request, _logger).AsFile();

            var returnedImage = new Bitmap(new MemoryStream(response.FileContents));
            var pixel = returnedImage.GetPixel(7, 7);
            Assert.That(pixel.Name, Is.EqualTo("fffe0000"));
        }

        [Test]
        public void Execute_ErrorIsThrown_LogsAndRethrows()
        {
            var request = ExpectedRequests.MapRequestForKey("foo");
            _sut = new MapCommand(null, null); // Will cause a null ref exception.

            Assert.Throws<NullReferenceException>(() => _sut.Execute(null, _logger));

            Assert.That(_logger.Entries.Count, Is.EqualTo(1));
        }
    }
}