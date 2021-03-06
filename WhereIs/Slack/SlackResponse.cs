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
        
        public SlackResponse(string result, string imageUrl)
        {
            text = result;
            attachments = new List<SlackAttachment>
            {
                new SlackAttachment
                {
                    text = "Here's a heatmap of availability",
                    image_url = imageUrl
                }
            }.ToArray();
        }
        public SlackResponse(Location result, string imageUrl)
        {
            text = result.Name;
            attachments = new List<SlackAttachment>
            {
                new SlackAttachment
                {
                    text = $"{result.Name} is marked on the map.",
                    image_url = imageUrl
                }
            }.ToArray();
        }

        public static SlackResponse NotFound() => new SlackResponse("Sorry! We can't find that place either.");
        public static SlackResponse NoLocationProvided() => new SlackResponse("Sorry! You need to specify a location.");
        public JsonResult AsJson() => new JsonResult(this);
    }
}