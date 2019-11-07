namespace WhereIs.FindingPlaces
{
    public class ImageLocation : Coord
    {
        public string Map { get; set; }

        // Required for deserialization
        public ImageLocation()
        {
        }

        public ImageLocation(int x, int y, string map = "map")
        {
            Map = map;
            X = x;
            Y = y;
        }
    }
}