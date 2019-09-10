namespace WhereIs
{
    public class Location
    {
        public string Name { get; set; }
        public string Key => Name.ToLower();

        public static Location NotFound { get; } = new Location {Name = "NOTFOUND"};
    }
}