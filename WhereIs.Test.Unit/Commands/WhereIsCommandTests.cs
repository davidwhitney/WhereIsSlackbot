using System.Collections.Generic;
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
            var locationFinder = new LocationFinder(new List<Location>
            {
                new Location("Foo"),
                new Location("Bar"),
                new Location("Baz")
            });
            _sut = new WhereIsCommand(locationFinder, new UrlHelper(fakeConfiguration), fakeConfiguration);
        }

        [Test]
        public void Invoke_NoValidDetailsFound_ReturnsFriendlyError()
        {
            var req = new SlackRequest();

            var response = _sut.Invoke(req);

            Assert.That(response.text, Is.EqualTo("Sorry! We can't find that place either."));
        }

        [Test]
        public void Invoke_KnownLocation_ReturnsLocation()
        {
            var req = new SlackRequest {Text = "Foo"};

            var response = _sut.Invoke(req);

            Assert.That(response.text, Is.EqualTo("Foo"));
        }

        [Test]
        public void Invoke_KnownLocation_ReturnsLocationMap()
        {
            var req = new SlackRequest {Text = "Foo"};

            var response = _sut.Invoke(req);

            Assert.That(response.attachments[0].image_url, Is.EqualTo("https://localhost/api/Map?code=key123&key=foo"));
        }
    }
}
