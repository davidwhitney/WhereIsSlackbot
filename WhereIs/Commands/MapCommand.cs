using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using WhereIs.FindingPlaces;

namespace WhereIs.Commands
{
    public class MapCommand
    {
        private readonly LocationCollection _locations;

        public MapCommand(LocationCollection locations)
        {
            _locations = locations;
        }

        [FunctionName(nameof(Map))]
        public async Task<IActionResult> Map(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req,
            ILogger log,
            ExecutionContext context)
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

                var map = Path.Combine(context.FunctionAppDirectory, "App_Data", "Maps", $"{location.ImageLocation.Map}.png");
                var outputBytes = HighlightAreaInImage(map, location);

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

        private static byte[] HighlightAreaInImage(string path, Location location)
        {
            using (var rawMap = Image.Load<Rgba32>(path))
            {
                DrawHighlight(location, rawMap);
                return GetBytesForModifiedImage(rawMap);
            }
        }

        private static byte[] GetBytesForModifiedImage(Image rawMap)
        {
            using (var imgStream = new MemoryStream())
            {
                rawMap.SaveAsJpeg(imgStream);
                imgStream.Position = 0;
                var outputBytes = imgStream.GetBuffer();
                imgStream.Close();
                return outputBytes;
            }
        }

        private static void DrawHighlight(Location location, Image<Rgba32> rawMap)
        {
            const int sizeOfHighlight = 20;

            var xRange = Enumerable.Range(location.ImageLocation.X - sizeOfHighlight, sizeOfHighlight * 2).ToList();
            var yRange = Enumerable.Range(location.ImageLocation.Y - sizeOfHighlight, sizeOfHighlight * 2).ToList();

            xRange.RemoveAll(x => x < 0 || x > rawMap.Width);
            yRange.RemoveAll(y => y < 0 || y > rawMap.Height);

            foreach(var x in xRange)
            {
                foreach (var y in yRange)
                {
                    rawMap[x, y] = Rgba32.Red;
                }
            }
        }
    }
}