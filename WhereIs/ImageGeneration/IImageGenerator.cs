using WhereIs.FindingPlaces;

namespace WhereIs.ImageGeneration
{
    public interface IImageGenerator
    {
        byte[] GetImageFor(Location location);
    }
}