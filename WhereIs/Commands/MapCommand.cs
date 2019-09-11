using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
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

                var map = Path.Combine(context.FunctionAppDirectory, $"{location.ImageLocation.Map}.png");
                var outputBytes = HighlightAreaInImage(map, location);

                return new FileContentResult(outputBytes, "image/jpeg");
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                throw;
            }
        }

        private static byte[] HighlightAreaInImage(string path, Location location)
        {
            var rawMap = (Bitmap) Image.FromFile(path);
            
            DrawHighlight(location, rawMap);

            using (var imgStream = new MemoryStream())
            {
                var jpgEncoder = ImageCodecInfo.GetImageDecoders().FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);

                // Create an Encoder object based on the GUID  
                // for the Quality parameter category.  
                var myEncoder = Encoder.Quality;
                var myEncoderParameters = new EncoderParameters(1);
                var myEncoderParameter = new EncoderParameter(myEncoder, 80L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                rawMap.Save(imgStream, jpgEncoder, myEncoderParameters);
                imgStream.Position = 0;
                var outputBytes = imgStream.GetBuffer();
                imgStream.Close();
                return outputBytes;
            }
        }

        private static void DrawHighlight(Location location, Bitmap rawMap)
        {
            const int sizeOfHighlight = 10;

            var xRange = Enumerable.Range(location.ImageLocation.X - sizeOfHighlight, location.ImageLocation.X + sizeOfHighlight).ToList();
            var yRange = Enumerable.Range(location.ImageLocation.Y - sizeOfHighlight, location.ImageLocation.Y + sizeOfHighlight).ToList();

            xRange.RemoveAll(x => x < 0 || x > rawMap.Width);
            yRange.RemoveAll(y => y < 0 || y > rawMap.Height);

            foreach(var x in xRange)
            {
                foreach (var y in yRange)
                {
                    rawMap.SetPixel(x, y, Color.Red);
                }
            }
        }
    }
}