using WhereIs.FindingPlaces;
using WhereIs.ImageGeneration;

namespace WhereIs.Test.Unit.Fakes
{
    public class FakeGenerator : IImageGenerator
    {
        public byte[] Returns { get; set; }
        public int Called { get; set; }

        public byte[] GetImageFor(ImageLocation location)
        {
            Called++;
            return Returns;
        }
    }
}