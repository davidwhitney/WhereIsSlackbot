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
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var command = new WhereIsCommand();
            var response = await command.Invoke(req);
            return new JsonResult(response);
        }
    }
}