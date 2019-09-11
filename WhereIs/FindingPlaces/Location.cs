namespace WhereIs.FindingPlaces
{
    public class Location
    {
        public string Name { get; set; }
        public ImageLocation ImageLocation { get; }
        public string Key => Name.ToLower();

        public Location()
        {
        }

        public Location(string name, ImageLocation imageLocation = null)
        {
            Name = name;
            ImageLocation = imageLocation ?? new ImageLocation(0, 0);
        }

        public static Location NotFound { get; } = new Location {Name = "NOTFOUND"};
    }

    public static class LocationExtensions
    {
        public static bool IsNotFound(this Location src)
        {
            if (src == null) return true;
            return src == Location.NotFound;
        }
    }
}