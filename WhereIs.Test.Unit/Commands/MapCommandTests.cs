using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using NUnit.Framework;
using WhereIs.Commands;
using WhereIs.FindingPlaces;
using WhereIs.Test.Unit.Fakes;

namespace WhereIs.Test.Unit.Commands
{
    [TestFixture]
    public class MapCommandTests
    {
        private MapCommand _sut;
        private FakeLogger _logger;
        private LocationCollection _knownLocations;
        private ExecutionContext _ctx;

        [SetUp]
        public void SetUp()
        {
            _logger = new FakeLogger();
            _knownLocations = new LocationCollection
            {
                new Location("Foo", new ImageLocation(10, 10)),
                new Location("Bar"),
                new Location("Baz")
            };
            _ctx = new ExecutionContext {FunctionAppDirectory = Environment.CurrentDirectory};
            _sut = new MapCommand(_knownLocations);
        }

        [Test]
        public async Task Map_NoValidKeyProvided_Returns404()
        {
            var request = ExpectedRequests.MapRequestForKey(null);

            var response = await _sut.Map(request, _logger, _ctx);

            Assert.That(response, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Invoke_ForKnownKey_ReturnsJpeg()
        {
            var request = ExpectedRequests.MapRequestForKey("foo");

            var response = await _sut.Map(request, _logger, _ctx).AsFile();

            Assert.That(response.ContentType, Is.EqualTo("image/jpeg"));
        }

        [Test]
        public async Task Invoke_ForKnownKey_ReturnsModifiedImage()
        {
            var path = Path.Combine(_ctx.FunctionAppDirectory, "map.png");
            var defaultMap = File.ReadAllBytes(path);
            var request = ExpectedRequests.MapRequestForKey("foo");

            var response = await _sut.Map(request, _logger, _ctx).AsFile();

            Assert.That(defaultMap.SequenceEqual(response.FileContents), Is.False);
        }

        [Test]
        public async Task Invoke_ForKnownKeyPlaces_MarkerCoveringLocationOnMapInRed()
        {
            var request = ExpectedRequests.MapRequestForKey("foo");

            var response = await _sut.Map(request, _logger, _ctx).AsFile();

            var returnedImage = new Bitmap(new MemoryStream(response.FileContents));
            var pixel = returnedImage.GetPixel(10, 10);
            Assert.That(pixel.Name, Is.EqualTo("fffe0000"));
        }

        [Test]
        public async Task Invoke_ForKnownKeyPlaces_MarkerCoveringLocationOnMapInRedIsBiggerThanOnePixel()
        {
            var request = ExpectedRequests.MapRequestForKey("foo");

            var response = await _sut.Map(request, _logger, _ctx).AsFile();

            var returnedImage = new Bitmap(new MemoryStream(response.FileContents));
            var pixel = returnedImage.GetPixel(7, 7);
            Assert.That(pixel.Name, Is.EqualTo("fffe0000"));

            var temp = Path.GetTempFileName();
            File.WriteAllBytes(temp, response.FileContents);
            Console.WriteLine(temp);
        }
    }
}