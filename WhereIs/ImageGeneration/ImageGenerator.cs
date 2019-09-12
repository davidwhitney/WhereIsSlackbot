using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using WhereIs.FindingPlaces;
using Configuration = WhereIs.Infrastructure.Configuration;

namespace WhereIs.ImageGeneration
{
    public class ImageGenerator : IImageGenerator
    {
        private readonly Configuration _config;
        public ImageGenerator(Configuration config) => _config = config;

        public byte[] GetImageFor(ImageLocation location)
        {
            var map = Path.Combine(_config.MapPath, $"{location.Map}.png");
            return HighlightAreaInImage(map, location);
        }

        private static byte[] HighlightAreaInImage(string path, ImageLocation location)
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

        private static void DrawHighlight(ImageLocation location, Image<Rgba32> rawMap)
        {
            const int sizeOfHighlight = 20;

            var xRange = Enumerable.Range(location.X - sizeOfHighlight, sizeOfHighlight * 2).ToList();
            var yRange = Enumerable.Range(location.Y - sizeOfHighlight, sizeOfHighlight * 2).ToList();

            xRange.RemoveAll(x => x < 0 || x > rawMap.Width);
            yRange.RemoveAll(y => y < 0 || y > rawMap.Height);

            foreach (var x in xRange)
            {
                foreach (var y in yRange)
                {
                    rawMap[x, y] = Rgba32.Red;
                }
            }
        }
    }
}