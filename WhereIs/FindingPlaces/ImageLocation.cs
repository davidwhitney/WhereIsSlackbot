namespace WhereIs.FindingPlaces
{
    public class ImageLocation
    {
        public string Map { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

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