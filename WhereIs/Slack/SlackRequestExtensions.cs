using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WhereIs.Slack
{
    public static class SlackRequestExtensions
    {
        public static async Task<SlackRequest> ReadSlackRequest(this HttpRequest req)
        {
            using (var stream = new StreamReader(req.Body))
            {
                var requestBody = await stream.ReadToEndAsync();
                return PayloadMapper.Map<SlackRequest>(requestBody);
            }
        }
    }
}