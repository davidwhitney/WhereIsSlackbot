using System.Collections.Generic;

namespace WhereIs.Slack
{
    public class SlackResponse
    {
        public string text { get; set; }
        public SlackAttachment[] attachments { get; set; }

        public SlackResponse()
        {
        }

        public SlackResponse(string message) => text = message;
    }
}