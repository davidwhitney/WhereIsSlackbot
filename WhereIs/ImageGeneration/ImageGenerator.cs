using System.Collections.Generic;
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
            return HighlightMap(location.Map, new[] {new Highlight(location, Rgba32.Red)});
        }

        public byte[] HighlightMap(string location, IEnumerable<Highlight> highlights)
        {
            var map = Path.Combine(_config.MapPath, $"{location}.png");
            return HighlightAreaInImage(map, highlights);
        }

        private static byte[] HighlightAreaInImage(string path, IEnumerable<Highlight> highlights)
        {
            using (var rawMap = Image.Load<Rgba32>(path))
            {
                foreach (var loc in highlights)
                {
                    DrawHighlight(loc, rawMap);
                }

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

        private static void DrawHighlight(Highlight highlight, Image<Rgba32> rawMap)
        {
            const int sizeOfHighlight = 20;

            var xRange = Enumerable.Range(highlight.Location.X - sizeOfHighlight, sizeOfHighlight * 2).ToList();
            var yRange = Enumerable.Range(highlight.Location.Y - sizeOfHighlight, sizeOfHighlight * 2).ToList();

            xRange.RemoveAll(x => x < 0 || x > rawMap.Width);
            yRange.RemoveAll(y => y < 0 || y > rawMap.Height);

            foreach (var x in xRange)
            {
                foreach (var y in yRange)
                {
                    rawMap[x, y] = highlight.Colour;
                }
            }
        }
    }
}