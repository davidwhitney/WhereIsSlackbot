using SixLabors.ImageSharp.PixelFormats;
using WhereIs.FindingPlaces;

namespace WhereIs.ImageGeneration
{
    public class Highlight
    {
        public Coord Location { get; set; }
        public Rgba32 Colour { get; set; }

        public Highlight(Coord location, Rgba32 colour)
        {
            Location = location;
            Colour = colour;
        }
    }
}