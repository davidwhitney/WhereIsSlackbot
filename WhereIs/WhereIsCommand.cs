using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WhereIs.Slack;

namespace WhereIs
{
    public class WhereIsCommand
    {
        private readonly ILocationFinder _finder;
        private readonly IUrlHelper _urlHelper;

        public WhereIsCommand(ILocationFinder finder, IUrlHelper urlHelper, IConfiguration config)
        {
            _finder = finder ?? new LocationFinder();
            _urlHelper = urlHelper ?? new UrlHelper(config);
        }

        [FunctionName("WhereIs")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log,
            ExecutionContext context)
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
            var imageUrl = _urlHelper.ImageFor(result.Key);

            return new SlackResponse
            {
                text = "I know where that is!",
                attachments = new List<SlackAttachment>
                {
                    new SlackAttachment
                    {
                        text = result.Key,
                        image_url = imageUrl
                    }
                }.ToArray()
            };
        }
    }
}