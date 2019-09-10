using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using WhereIs.Slack;

namespace WhereIs
{
    public class WhereIsCommand
    {
        private readonly UrlHelper _urlHelper;

        public WhereIsCommand(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public async Task<SlackResponse> Invoke(HttpRequest req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = PayloadMapper.Map(requestBody);
            var finder = new LocationFinder();

            var result = finder.Find(request.Text);
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