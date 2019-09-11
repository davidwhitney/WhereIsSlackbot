using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WhereIs.FindingPlaces;

namespace WhereIs.Slack
{
    public class SlackResponse
    {
        public string text { get; set; }
        public SlackAttachment[] attachments { get; set; } = new SlackAttachment[0];

        public SlackResponse(string message) => text = message;
        public SlackResponse(Location result, string imageUrl)
        {
            text = result.Name;
            attachments = new List<SlackAttachment>
            {
                new SlackAttachment
                {
                    text = $"{result.Name} is marked on the map. " + imageUrl,
                    image_url = imageUrl
                }
            }.ToArray();
        }

        public JsonResult AsJson() => new JsonResult(this);
    }
}