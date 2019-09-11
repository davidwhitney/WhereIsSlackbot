using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace WhereIs.Test.Unit.Fakes
{
    public class SlackWebHookRequest
    {
        public static DefaultHttpRequest WithText(string text, string command = "/whereis")
        {
            var encodedText = HttpUtility.UrlEncode(text);
            var encodedCommand = HttpUtility.UrlEncode(command)?.Replace("%2f", "/"); // Slack is weird.
            var body = $"token=gIkuvaNzQIHg97ATvDxqgjtO&channel_name=test&command={encodedCommand}&text={encodedText}";

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(body);
            writer.Flush();
            stream.Position = 0;

            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(),
                Body = stream
            };
        }
    }
}