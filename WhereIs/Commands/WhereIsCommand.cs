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

namespace WhereIs.Commands
{
    public class WhereIsCommand
    {
        private readonly ILocationFinder _finder;
        private readonly IUrlHelper _urlHelper;

        public WhereIsCommand(ILocationFinder finder, IUrlHelper urlHelper)
        {
            _finder = finder;
            _urlHelper = urlHelper;
        }

        [FunctionName("WhereIs")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
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
                    return new SlackResponse("Sorry! We can't find that place either.").AsJson();
                }

                var imageUrl = _urlHelper.ImageFor(result.Key);
                log.LogDebug("Returning generated image url: " + imageUrl);

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