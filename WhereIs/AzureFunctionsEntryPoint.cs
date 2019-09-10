using System;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace WhereIs
{
    public static class AzureFunctionsEntryPoint
    {
        [FunctionName("WhereIs")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            try
            {
                var urlHelper = new UrlHelper(context);
                var command = new WhereIsCommand(urlHelper);
                var response = await command.Invoke(req);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                throw;
            }
        }

        [FunctionName("Map")]
        public static async Task<IActionResult> Map(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            try
            {
                var bytes = File.ReadAllBytes("map.png");
                return new FileContentResult(bytes, "	image/png");
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                throw;
            }
        }
    }
}