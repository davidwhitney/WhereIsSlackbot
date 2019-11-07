namespace WhereIs.Infrastructure
{
    public interface IUrlHelper
    {
        string ImageFor(string locationKey);
        string CapacityImageFor(string locationKey);
    }
}