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
}