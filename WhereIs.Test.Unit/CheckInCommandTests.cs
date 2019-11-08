using System;
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

            var response = await _sut.Execute(request, _logger) as JsonResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test]
        public async Task Run_KnownLocationRequested_ThanksUserAtTheEnd()
        {
            var request = ExpectedRequests.CheckInFor("gracechurch::245-210");

            var response = await _sut.Execute(request, _logger) as JsonResult;

            Assert.That(response.Value.ToString(), Is.EqualTo("{ message = Thanks for checking in! }"));
        }

        [Test]
        public async Task Run_KnownLocationRequested_LogsCheckinRequest()
        {
            var request = ExpectedRequests.CheckInFor("gracechurch::245-210");

            var response = await _sut.Execute(request, _logger) as JsonResult;

            Assert.That(_capacityService.Called, Is.True);
        }

        [Test]
        public void Execute_ErrorIsThrown_LogsAndRethrows()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _sut.Execute(null, _logger));

            Assert.That(_logger.Entries.Count, Is.EqualTo(1));
        }
    }
}
