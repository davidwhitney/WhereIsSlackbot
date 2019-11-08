using System.Collections.Generic;
using NUnit.Framework;
using WhereIs.CapacityMonitoring;

namespace WhereIs.Test.Unit.CapacityMonitoring
{
    [TestFixture]
    public class CapacityServiceTests
    {
        private CapacityService _sut;
        private FakeCapacityRepo _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new FakeCapacityRepo();
            _sut = new CapacityService(_repository);
        }

        [Test]
        public void CheckIn_IncrementsNumberAgainstProvidedLocationKey()
        {
            _sut.CheckIn("gracechurch::245-210");

            var occupiedCount = _sut.NumberOfDesksOccupiedForLocation("gracechurch");

            Assert.That(occupiedCount, Is.EqualTo(1));
        }
    }

    public class FakeCapacityRepo : ICapacityRepository
    {
        public Dictionary<string, int> Storage { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> Load() => Storage;
        public void Save(Dictionary<string, int> state) => Storage = state;
    }
}
