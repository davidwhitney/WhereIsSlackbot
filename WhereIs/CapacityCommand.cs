using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using WhereIs.FindingPlaces;
using WhereIs.Slack;
using IUrlHelper = WhereIs.Infrastructure.IUrlHelper;

namespace WhereIs
{
    public class CapacityCommand
    {
        private readonly LocationCollection _locations;
        private readonly IUrlHelper _urlHelper;

        public CapacityCommand(LocationCollection locations, IUrlHelper urlHelper)
        {
            _locations = locations;
            _urlHelper = urlHelper;
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


                var result = "There are 6 of 6 free desks in Gracechurch.";
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