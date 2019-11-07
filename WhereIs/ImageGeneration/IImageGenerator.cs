using System.Collections.Generic;
using WhereIs.FindingPlaces;

namespace WhereIs.ImageGeneration
{
    public interface IImageGenerator
    {
        byte[] GetImageFor(ImageLocation location);
        byte[] HighlightMap(string map, IEnumerable<Highlight> highlights);
    }
}