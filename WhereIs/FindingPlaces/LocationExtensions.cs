namespace WhereIs.FindingPlaces
{
    public static class LocationExtensions
    {
        public static bool IsNotFound(this Location src)
        {
            if (src == null) return true;
            return src == Location.NotFound;
        }
    }
}