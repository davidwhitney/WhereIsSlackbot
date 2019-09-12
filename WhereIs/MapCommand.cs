using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using WhereIs.FindingPlaces;
using WhereIs.ImageGeneration;
using Configuration = WhereIs.Infrastructure.Configuration;

namespace WhereIs
{
    public class MapCommand
    {
        private readonly LocationCollection _locations;
        private readonly IMemoryCache _cache;
        private readonly ImageGenerator _generator;

        public MapCommand(LocationCollection locations, Configuration config, IMemoryCache cache)
        {
            _locations = locations;
            _cache = cache;
            _generator = new ImageGenerator(config);
        }

        [FunctionName("Map")]
        public IActionResult Execute([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            try
            {
                var mapKey = req.Query["key"].FirstOrDefault();
                if (mapKey == null)
                {
                    return new NotFoundResult();
                }

                var location = _locations.SingleOrDefault(x => x.Key == mapKey);
                if (location == null)
                {
                    return new NotFoundResult();
                }

                var outputBytes = _cache.GetOrCreate(location.Key, entry => _generator.GetImageFor(location));

                return new FileContentResult(outputBytes, "image/jpeg")
                {
                    FileDownloadName = $"map_{mapKey}.jpg"
                };
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                throw;
            }
        }

    }
}