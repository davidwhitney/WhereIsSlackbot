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
        private readonly ILocationFinder _finder;
        private readonly IUrlHelper _urlHelper;

        public CapacityCommand(ILocationFinder finder, IUrlHelper urlHelper)
        {
            _finder = finder;
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

                var result = _finder.Find(request.Text);
                if (result.IsNotFound())
                {
                    return SlackResponse.NotFound().AsJson();
                }

                var imageUrl = _urlHelper.ImageFor(result.Key);
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