using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using WhereIs.CapacityMonitoring;
using WhereIs.FindingPlaces;
using WhereIs.Infrastructure;
using WhereIs.Slack;
using IUrlHelper = WhereIs.Infrastructure.IUrlHelper;

namespace WhereIs
{
    public class CapacityCommand
    {
        private readonly LocationCollection _locations;
        private readonly IUrlHelper _urlHelper;
        private readonly ICapacityService _capacityService;

        public CapacityCommand(LocationCollection locations, IUrlHelper urlHelper, ICapacityService capacityService)
        {
            _locations = locations;
            _urlHelper = urlHelper;
            _capacityService = capacityService;
        }

        [FunctionName("Capacity")]
        public async Task<IActionResult> Execute([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            try
            {
                var request = await req.ReadSlackRequest();
                if (string.IsNullOrWhiteSpace(request.Text))
                {
                    return SlackResponse.NoLocationProvided().AsJson();
                }

                var location = new LocationFromRequest(request.Text);

                var totalAvailableSeats = _locations.TotalCapacityOf(location);
                var filledSeats = _capacityService.NumberOfDesksOccupiedForLocation(location);

                var result = $"There are {filledSeats} of {totalAvailableSeats} desks used in {request.Text}.";
                var imageUrl = _urlHelper.CapacityImageFor(location);
                return new SlackResponse(result, imageUrl).AsJson();
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                throw;
            }
        }
    }
}