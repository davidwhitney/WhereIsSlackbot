﻿using NUnit.Framework;

namespace WhereIs.Test.Unit
{
    [TestFixture]
    public class LocationFinderTests
    {
        private LocationFinder _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new LocationFinder();
        }

        [Test]
        public void Find_PlaceDoesNotExist_ReturnsErrorMessage()
        {
            var result = _sut.Find("junk-place");

            Assert.That(result, Is.EqualTo(Location.NotFound));
        }

        [Test]
        public void Find_PlaceKnown_ReturnsNotNullLocationResult()
        {
            var result = _sut.Find("Place that exists");

            Assert.That(result, Is.InstanceOf<Location>());
            Assert.That(result, Is.Not.EqualTo(Location.NotFound));
        }

        [Test]
        public void Find_PlaceKnown_LocationResultContainsLocation()
        {
            var result = _sut.Find("Place that exists");

            Assert.That(result.Name, Is.EqualTo("Place that exists"));
        }

        [Test]
        public void Find_PlaceKnown_IgnoresCase()
        {
            var result = _sut.Find("place tHat exists");

            Assert.That(result.Name, Is.EqualTo("Place that exists"));
        }

        [TestCase("place tcat exists")]
        [TestCase("place tcbt exists")]
        [TestCase("place tcbh exists")]
        [TestCase("place tcbh cxists")]
        public void Find_FindingAMisspellingOfAKnownPlace_SuggestsMatchesWhenNoExactMatchIsPresent(string supportedMisspelling)
        {
            var result = _sut.Find(supportedMisspelling);

            Assert.That(result.Name, Is.EqualTo("Place that exists"));
        }

        [Test]
        public void Find_AllFuzzySpellingsTooDifferent_ReturnsNotFound()
        {
            var result = _sut.Find("place tcbh cxasts");

            Assert.That(result, Is.EqualTo(Location.NotFound));
        }

    }
}