using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using WhereIs.Slack;

namespace WhereIs
{
    public class WhereIsCommand
    {
        private readonly LocationFinder _finder;
        private readonly UrlHelper _urlHelper;

        public WhereIsCommand(LocationFinder finder, UrlHelper urlHelper)
        {
            _finder = finder;
            _urlHelper = urlHelper;
        }

        [FunctionName("WhereIs")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            try
            {
                var requestBody = new StreamReader(req.Body).ReadToEndAsync().GetAwaiter().GetResult();
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