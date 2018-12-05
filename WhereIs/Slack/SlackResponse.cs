namespace WhereIs.Slack
{
    public class SlackResponse
    {
        public string text { get; set; }
        public SlackAttachments attachments { get; set; } = new SlackAttachments();
    }
}