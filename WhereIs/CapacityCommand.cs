using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using WhereIs.CapacityMonitoring;
using WhereIs.FindingPlaces;
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
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var request = PayloadMapper.Map<SlackRequest>(requestBody);

                if (string.IsNullOrWhiteSpace(request.Text))
                {
                    return SlackResponse.NoLocationProvided().AsJson();
                }

                var location = request.Text.ToLower().Trim();
                var totalAvailableSeats = _locations.Where(x => x.Name.StartsWith(location)).Sum(x => x.Capacity);
                var filledSeats = _capacityService.NumberOfDesksOccupiedForLocation(location);

                var result = $"There are {filledSeats} of {totalAvailableSeats} free desks in Gracechurch.";
                //var imageUrl = _urlHelper.ImageFor(result.Key);
                return new SlackResponse(result).AsJson();
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                throw;
            }
        }
    }
}