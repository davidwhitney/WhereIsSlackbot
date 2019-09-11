using System.Collections.Generic;

namespace WhereIs.FindingPlaces
{
    public static class HardcodedLocations
    {
        public static List<Location> Items = new List<Location>
        {
            new Location {Name = "Test"},
            new Location {Name = "Place that exists"},

            // Real places

            new Location("Plato", new ImageLocation(386, 105)),
            new Location("Aristotle", new ImageLocation(440, 105)),
            new Location("Locke", new ImageLocation(488, 105)),
        };
    }
}