using System.Linq;
using System.Reflection;
using NUnit.Framework;
using WhereIs.FindingPlaces;

namespace WhereIs.Test.Unit.FindingPlaces
{
    [TestFixture]
    public class ImageLocationTests
    {
        [Test]
        public void DefaultConstructor_Exists_ForSerializer()
        {
            var type = typeof(ImageLocation);

            var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            Assert.That(ctors.Any(x=>x.GetParameters().Length == 0));
        }

        [Test]
        public void CanSetAllProperties_BecauseSerializerNeedsThem_AndThisTookAgesToDiagnose()
        {
            var sut = new ImageLocation(1, 2, "abc");

            sut.X = 9;
            sut.Y = 10;
            sut.Map = "blah";

            Assert.That(sut.X, Is.EqualTo(9));
            Assert.That(sut.Y, Is.EqualTo(10));
            Assert.That(sut.Map, Is.EqualTo("blah"));
        }
    }
}
