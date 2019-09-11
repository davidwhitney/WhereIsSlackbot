using System;
using System.Collections.Generic;
using System.Text;
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
            var sut = new LocationRepository();

            var items = sut.Load(Environment.CurrentDirectory);

            Assert.That(items, Is.Not.Null);
        }
    }
}
