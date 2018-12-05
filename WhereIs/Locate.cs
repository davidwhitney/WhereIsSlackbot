using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using WhereIs.Slack;

namespace WhereIs
{
    public static class Locate
    {
        [FunctionName("Locate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var payload = PayloadMapper.Map(requestBody);


            var resp = new OkObjectResult(new SlackResponse
            {
                text = "I know where that is!",
                attachments =
                {
                    text = "Describing the image right here...",
                    image_url = "https://a.slack-edge.com/ae57/img/slack_api_logo.png"
                }
            });
            resp.ContentTypes.Clear();
            resp.ContentTypes.Add(new MediaTypeHeaderValue("application/json"));
            return resp;
        }
    }

}
