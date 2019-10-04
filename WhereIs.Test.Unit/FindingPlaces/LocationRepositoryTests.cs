using System;
using NUnit.Framework;
using WhereIs.FindingPlaces;

namespace WhereIs.Test.Unit.FindingPlaces
{
    [TestFixture]
    public class LocationRepositoryTests
    {
        [Test]
        public void Load_WithCorrectPath_CanDeserializeItems()
        {
            var sut = new LocationRepository(Environment.CurrentDirectory);

            var items = sut.Load();

            Assert.That(items, Is.Not.Null);
        }

        [Test]
        public void Load_WithCorrectPath_CanDeserializeItems_DataIsCorrect()
        {
            var sut = new LocationRepository(Environment.CurrentDirectory);

            var items = sut.Load();

            Assert.That(items[0], Is.Not.Null);
            Assert.That(items[0].ImageLocation, Is.Not.Null);
            Assert.That(items[0].ImageLocation.Map, Is.Not.Null);
        }
    }
}
