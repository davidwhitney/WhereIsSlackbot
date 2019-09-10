using System.Collections.Generic;

namespace WhereIs.Slack
{
    public class SlackResponse
    {
        public string text { get; set; }
        public List<SlackAttachments> attachments { get; set; } = new List<SlackAttachments>();
    }
}