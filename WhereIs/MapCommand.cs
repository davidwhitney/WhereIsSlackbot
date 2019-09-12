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

namespace WhereIs
{
    public class MapCommand
    {
        private readonly LocationCollection _locations;
        private readonly IMemoryCache _cache;
        private readonly IImageGenerator _generator;

        public MapCommand(LocationCollection locations, IImageGenerator generator, IMemoryCache cache)
        {
            _locations = locations ?? throw new ArgumentNullException(nameof(locations));
            _generator = generator ?? throw new ArgumentNullException(nameof(generator));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        [FunctionName("Map")]
        public IActionResult Execute([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            try
            {
                var mapKey = req.Query["key"].FirstOrDefault();
                var location = _locations.SingleOrDefault(x => x.Key == mapKey);
                if (location == null)
                {
                    return new NotFoundResult();
                }

                var outputBytes = _cache.GetOrCreate(location.Key, entry => _generator.GetImageFor(location.ImageLocation));
                return new FileContentResult(outputBytes, "image/jpeg");
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                throw;
            }
        }

    }
}