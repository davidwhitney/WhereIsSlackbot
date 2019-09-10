using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using WhereIs.Slack;

namespace WhereIs
{
    public class WhereIsCommand
    {
        private readonly SlackRequest _request;
        private readonly LocationFinder _finder;
        private readonly UrlHelper _urlHelper;

        public WhereIsCommand(SlackRequest request, LocationFinder finder, UrlHelper urlHelper)
        {
            _request = request;
            _finder = finder;
            _urlHelper = urlHelper;
        }

        public SlackResponse Invoke(HttpRequest req)
        {
            var result = _finder.Find(_request.Text);
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