using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using WhereIs.CapacityMonitoring;
using WhereIs.Infrastructure;

namespace WhereIs
{
    public class CheckInCommand
    {
        private readonly ICapacityService _capacityService;

        public CheckInCommand(ICapacityService capacityService)
        {
            _capacityService = capacityService;
        }

        [FunctionName("CheckIn")]
        public async Task<IActionResult> Execute([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            try
            {
                var rawLocation = req.Query.ContainsKey("location") ? req.Query["location"].FirstOrDefault() : null;
                var location = new LocationFromRequest(rawLocation);
                if (!location.IsValid())
                {
                    return new BadRequestResult();
                }

                _capacityService.CheckIn(location);
                return new JsonResult(new {message = "Thanks for checking in!"}) {StatusCode = 200};
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                throw;
            }
        }
    }
}