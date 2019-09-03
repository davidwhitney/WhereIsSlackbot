using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WhereIs.Slack;

namespace WhereIs
{
    public class WhereIsMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            var request = PayloadMapper.Map(requestBody);
            var finder = new LocationFinder();
            var result = finder.Find(request.Text);

            var slackResponse = new SlackResponse
            {
                text = "I know where that is!",
                attachments =
                {
                    text = "Describing the image right here...",
                    image_url = "https://a.slack-edge.com/ae57/img/slack_api_logo.png"
                }
            };

            var json = JsonConvert.SerializeObject(slackResponse);
            await context.Response.WriteAsync(json);
        }
    }
}