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
                BlobCredentials = "DefaultEndpointsProtocol=https;AccountName=whereischeckins;AccountKey=c4l+Wsn1rGRZfTIL1i1SZdoXjoMtIpHdYCBH+vZGk5jki9YDBJn+XQfcp0HSazrNjOs+JyENOkQOLcjGxsw5/g==;EndpointSuffix=core.windows.net"
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
