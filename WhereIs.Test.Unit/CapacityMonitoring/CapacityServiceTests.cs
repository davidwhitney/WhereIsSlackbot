using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using WhereIs.CapacityMonitoring;

namespace WhereIs.Test.Unit.CapacityMonitoring
{
    [TestFixture]
    public class CapacityServiceTests
    {
        [Test]
        public void CheckIn_IncrementsNumberAgainstProvidedLocationKey()
        {
            var sut = new CapacityService();

            sut.CheckIn("gracechurch::245-210");
            var occupiedCount = sut.NumberOfDesksOccupiedForLocation("gracechurch");

            Assert.That(occupiedCount, Is.EqualTo(1));
        }

        [Test]
        public void CheckIn_InvalidCompoundKey_Throws()
        {
            var sut = new CapacityService();

            var ex = Assert.Throws<Exception>(() => sut.CheckIn("key-does-not-have-two-colons-in-it"));

            Assert.That(ex.Message, Is.EqualTo("Invalid key for checkin."));
        }
    }
}
