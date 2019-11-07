using Azure.Storage.Blobs;
using NUnit.Framework;
using WhereIs.CapacityMonitoring;
using WhereIs.Infrastructure;

namespace WhereIs.Test.Unit.CapacityMonitoring
{
    [TestFixture(Category = "Integration")]
    public class CapacityServiceTests
    {
        private CapacityService _sut;

        [SetUp]
        public void SetUp()
        {
            var factory = new CapacityRepository(new Configuration
            {
                BlobCredentials = "blob-key"
            });

            _sut = new CapacityService(factory);
        }

        [Test]
        public void CheckIn_IncrementsNumberAgainstProvidedLocationKey()
        {
            var before = _sut.NumberOfDesksOccupiedForLocation("gracechurch");

            _sut.CheckIn("gracechurch::245-210");
            var occupiedCount = _sut.NumberOfDesksOccupiedForLocation("gracechurch");

            Assert.That(occupiedCount, Is.EqualTo(before + 1));
        }
    }
}
