using System.Collections.Generic;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;

namespace WhereIs.Test.Unit.Fakes
{
    public class ExpectedRequests
    {
        public static DefaultHttpRequest WhereIsFor(string text, string command = "/whereis")
        {
            var encodedText = HttpUtility.UrlEncode(text);
            var encodedCommand = HttpUtility.UrlEncode(command)?.Replace("%2f", "/"); // Slack is weird.
            var body = $"token=gIkuvaNzQIHg97ATvDxqgjtO&channel_name=test&command={encodedCommand}&text={encodedText}";
            var stream = StreamContaining(body);

            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(),
                Body = stream
            };
        }

        public static DefaultHttpRequest CapacityFor(string text, string command = "/capacity")
        {
            var encodedText = HttpUtility.UrlEncode(text);
            var encodedCommand = HttpUtility.UrlEncode(command)?.Replace("%2f", "/"); // Slack is weird.
            var body = $"token=gIkuvaNzQIHg97ATvDxqgjtO&channel_name=test&command={encodedCommand}&text={encodedText}";
            var stream = StreamContaining(body);

            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(),
                Body = stream
            };
        }

        public static DefaultHttpRequest CheckInFor(string location)
        {
            var queryCollection = new QueryCollection(new Dictionary<string, StringValues>
            {
                {"location", new StringValues(location)}
            });

            return new DefaultHttpRequest(new DefaultHttpContext()) { Query = queryCollection };
        }

        public static DefaultHttpRequest MapRequestForKey(string key)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(new Dictionary<string, StringValues>
                {
                    {"code", new StringValues("someApiKey")},
                    {"key", new StringValues(key)}
                })
            };
        }

        public static DefaultHttpRequest HeatMapRequestForKey(string key)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(new Dictionary<string, StringValues>
                {
                    {"code", new StringValues("someApiKey")},
                    {"key", new StringValues(key)}
                })
            };
        }

        private static MemoryStream StreamContaining(string location)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(location);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}