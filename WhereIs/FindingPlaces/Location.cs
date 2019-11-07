using System.Web;

namespace WhereIs.FindingPlaces
{
    public class Location
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public ImageLocation ImageLocation { get; set; }
        public string Key => HttpUtility.UrlEncode(Name.ToLower());
        public string RawKey() => HttpUtility.UrlDecode(Key);

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
}