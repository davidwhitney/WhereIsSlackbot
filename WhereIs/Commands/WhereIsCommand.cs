using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var request = PayloadMapper.Map(requestBody);

                var response = Invoke(request);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                throw;
            }
        }

        public SlackResponse Invoke(SlackRequest request)
        {
            var result = _finder.Find(request.Text);
            if (result == Location.NotFound)
            {
                return new SlackResponse("Sorry! We can't find that place either.");
            }

            var imageUrl = _urlHelper.ImageFor(result.Key);

            return new SlackResponse
            {
                text = result.Name,
                attachments = new List<SlackAttachment>
                {
                    new SlackAttachment
                    {
                        text = imageUrl,
                        image_url = imageUrl
                    }
                }.ToArray()
            };
        }
    }
}