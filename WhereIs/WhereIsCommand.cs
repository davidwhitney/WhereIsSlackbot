using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WhereIs.Slack;

namespace WhereIs
{
    public class WhereIsCommand
    {
        public async Task<SlackResponse> Invoke(HttpRequest req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = PayloadMapper.Map(requestBody);
            var finder = new LocationFinder();
            var result = finder.Find(request.Text);

            return new SlackResponse
            {
                text = "I know where that is!",
                attachments =
                {
                    text = "Describing the image right here...",
                    image_url = "https://a.slack-edge.com/ae57/img/slack_api_logo.png"
                }
            };
        }
    }
}