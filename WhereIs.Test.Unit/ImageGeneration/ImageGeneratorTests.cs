using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using WhereIs.FindingPlaces;
using WhereIs.ImageGeneration;
using WhereIs.Infrastructure;

namespace WhereIs.Test.Unit.ImageGeneration
{
    [TestFixture]
    public class ImageGeneratorTests
    {
        private ImageGenerator _sut;
        private Configuration _config;

        [SetUp]
        public void SetUp()
        {
            _config = new Configuration { ApiKey = "key", UrlRoot = "http://something" };
            _sut = new ImageGenerator(_config);
        }

        [Test]
        public void GetImageFor_ForKnownKey_ReturnsModifiedImage()
        {
            var defaultMap = File.ReadAllBytes(Path.Combine(_config.MapPath, "gracechurch.png"));
            var location = new ImageLocation(1, 1, "gracechurch");

            var response = _sut.GetImageFor(location);

            Assert.That(defaultMap.SequenceEqual(response), Is.False);
        }

        [Test]
        public void GetImageFor_ForKnownKeyPlaces_MarkerCoveringLocationOnMapInRed()
        {
            var location = new ImageLocation(1, 1, "gracechurch");

            var response = _sut.GetImageFor(location);

            var pixel = new Bitmap(new MemoryStream(response)).GetPixel(10, 10);
            Assert.That(pixel.Name, Is.EqualTo("fffe0000"));
        }

        [Test]
        public void GetImageFor_ForKnownKeyPlaces_MarkerCoveringLocationOnMapInRedIsBiggerThanOnePixel()
        {
            var location = new ImageLocation(1, 1, "gracechurch");

            var response = _sut.GetImageFor(location);

            var pixel = new Bitmap(new MemoryStream(response)).GetPixel(7, 7);
            Assert.That(pixel.Name, Is.EqualTo("fffe0000"));
        }

    }
}