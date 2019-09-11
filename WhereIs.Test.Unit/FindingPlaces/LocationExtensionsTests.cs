using NUnit.Framework;
using WhereIs.FindingPlaces;

namespace WhereIs.Test.Unit.FindingPlaces
{
    [TestFixture]
    public class LocationExtensionsTests
    {
        [Test]
        public void IsNotFound_GivenNull_IsTrue() => Assert.That(LocationExtensions.IsNotFound(null), Is.True);
        [Test]
        public void IsNotFound_GivenNotFound_IsTrue() => Assert.That(Location.NotFound.IsNotFound(), Is.True);
        [Test]
        public void IsNotFound_GivenLocation_IsFalse() => Assert.That(new Location("blah").IsNotFound(), Is.False);
    }
}
