using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhereIs.Slack;

namespace WhereIs.Test.Unit.Commands
{
    public static class ActionResultExtensions
    {
        public static SlackResponse AsSlackResponse(this IActionResult src) => ((JsonResult) src).Value as SlackResponse;
        public static async Task<SlackResponse> AsSlackResponse(this Task<IActionResult> src) => (await src).AsSlackResponse();
        public static FileContentResult AsFile(this IActionResult src) => (FileContentResult) src;
    }
}