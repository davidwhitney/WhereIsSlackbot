namespace WhereIs.FindingPlaces
{
    public class ImageLocation
    {
        public string Map { get; }
        public int X { get; }
        public int Y { get; }

        public ImageLocation(int x, int y, string map = "map")
        {
            Map = map;
            X = x;
            Y = y;
        }
    }
}