using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WhereIs.Test.Unit.Fakes;

namespace WhereIs.Test.Unit
{
    [TestFixture]
    public class CheckInCommandTests
    {
        private CheckInCommand _sut;
        private FakeLogger _logger;
        private FakeCapacityService _capacityService;

        [SetUp]
        public void SetUp()
        {
            _logger = new FakeLogger();
            _capacityService = new FakeCapacityService();
            _sut = new CheckInCommand(_capacityService);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task Run_NoLocationRequested_ReturnsBadRequest(string messageContents)
        {
            var request = ExpectedRequests.CheckInFor(messageContents);

            var response = await _sut.Execute(request, _logger) as BadRequestResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }

        [Test]
        public async Task Run_KnownLocationRequested_ReturnsHintAsToAvailability()
        {
            var request = ExpectedRequests.CheckInFor("gracechurch::245-210");

            var response = await _sut.Execute(request, _logger) as OkResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }
    }
}
