using NUnit.Framework;
using WhereIs.FindingPlaces;

namespace WhereIs.Test.Unit.FindingPlaces
{
    [TestFixture]
    public class LevenshteinDistanceTests
    {
        [TestCase("word", "word", 0)]
        [TestCase("word", "worD", 1)]
        [TestCase("word", "wordd", 1)]
        [TestCase("word", "wor", 1)]
        [TestCase("word", "worddd", 2)]
        [TestCase("", "", 0)]
        [TestCase("", "a", 1)]
        [TestCase("a", "", 1)]
        public void Compute_ComputesDistanceBetweenWords(string first, string second, int expectedDistance)
        {
            Assert.That(LevenshteinDistance.Compute(first, second), Is.EqualTo(expectedDistance));
        }
    }
}
