using NUnit.Framework;
using WhereIs.Commands;
using WhereIs.FindingPlaces;
using WhereIs.Infrastructure;
using WhereIs.Slack;
using WhereIs.Test.Unit.Fakes;

namespace WhereIs.Test.Unit.Commands
{
    [TestFixture]
    public class WhereIsCommandTests
    {
        private WhereIsCommand _sut;

        [SetUp]
        public void SetUp()
        {
            var fakeConfiguration = new FakeConfiguration();
            _sut = new WhereIsCommand(new LocationFinder(), new UrlHelper(fakeConfiguration), fakeConfiguration);
        }

        [Test]
        public void Invoke_NoValidDetailsFound_ReturnsFriendlyError()
        {
            var req = new SlackRequest();

            var response = _sut.Invoke(req);

            Assert.That(response.text, Is.EqualTo("Sorry! We can't find that place either."));
        }
    }
}
