namespace WhereIs.FindingPlaces
{
    public class Location
    {
        public string Name { get; set; }
        public string Key => Name.ToLower();

        public Location()
        {
        }

        public Location(string name)
        {
            Name = name;
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