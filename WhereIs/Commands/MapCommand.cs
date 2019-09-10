using System;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace WhereIs.Commands
{
    public class MapCommand
    {
        [FunctionName("Map")]
        public async Task<IActionResult> Map(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            try
            {
                var path = Path.Combine(context.FunctionAppDirectory, "map.png");
                var bytes = File.ReadAllBytes(path);
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