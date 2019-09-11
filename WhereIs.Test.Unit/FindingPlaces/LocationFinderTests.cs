using System;
using NUnit.Framework;
using WhereIs.FindingPlaces;
using WhereIs.Test.Unit.Fakes;

namespace WhereIs.Test.Unit.FindingPlaces
{
    [TestFixture]
    public class LocationFinderTests
    {
        private LocationFinder _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new LocationFinder(new FakeLocationRepository(new LocationCollection
            {
                new Location("one"),
                new Location("Place that exists"),
                new Location("two")
            }));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Find_NullOrWhitespace_ReturnsErrorMessage(string searchTerm)
        {
            var result = _sut.Find(searchTerm, Environment.CurrentDirectory);

            Assert.That(result, Is.EqualTo(Location.NotFound));
        }

        [Test]
        public void Find_PlaceDoesNotExist_ReturnsErrorMessage()
        {
            var result = _sut.Find("junk-place", Environment.CurrentDirectory);

            Assert.That(result, Is.EqualTo(Location.NotFound));
        }

        [Test]
        public void Find_PlaceKnown_ReturnsNotNullLocationResult()
        {
            var result = _sut.Find("Place that exists", Environment.CurrentDirectory);

            Assert.That(result, Is.InstanceOf<Location>());
            Assert.That(result, Is.Not.EqualTo(Location.NotFound));
        }

        [Test]
        public void Find_PlaceKnown_LocationResultContainsLocation()
        {
            var result = _sut.Find("Place that exists", Environment.CurrentDirectory);

            Assert.That(result.Name, Is.EqualTo("Place that exists"));
        }

        [Test]
        public void Find_PlaceKnown_IgnoresCase()
        {
            var result = _sut.Find("place tHat exists", Environment.CurrentDirectory);

            Assert.That(result.Name, Is.EqualTo("Place that exists"));
        }

        [TestCase("place tcat exists")]
        [TestCase("place tcbt exists")]
        [TestCase("place tcbh exists")]
        [TestCase("place tcbh cxists")]
        public void Find_FindingAMisspellingOfAKnownPlace_SuggestsMatchesWhenNoExactMatchIsPresent(string supportedMisspelling)
        {
            var result = _sut.Find(supportedMisspelling, Environment.CurrentDirectory);

            Assert.That(result.Name, Is.EqualTo("Place that exists"));
        }

        [Test]
        public void Find_AllFuzzySpellingsTooDifferent_ReturnsNotFound()
        {
            var result = _sut.Find("place tcbh cxasts", Environment.CurrentDirectory);

            Assert.That(result, Is.EqualTo(Location.NotFound));
        }

    }
}