using System.Collections.Generic;

namespace WhereIs.FindingPlaces
{
    public class LocationCollection : List<Location>
    {
        public static LocationCollection Defaults = new LocationCollection
        {
            new Location {Name = "Test"},
            new Location {Name = "Place that exists"},

            // Real places

            new Location("Ben Jones", new ImageLocation(300, 105)),
            new Location("David Whitney", new ImageLocation(220, 240)),

            new Location("Plato", new ImageLocation(386, 105)),
            new Location("Aristotle", new ImageLocation(440, 105)),
            new Location("Locke", new ImageLocation(488, 105)),

            new Location("Kitchen", new ImageLocation(590, 325)),
            new Location("Open Space", new ImageLocation(400, 575)),

            new Location("Austen", new ImageLocation(335, 700)),
            new Location("Shakespeare", new ImageLocation(585, 795)),

            new Location("Freud", new ImageLocation(475, 700)),
            new Location("Socrates", new ImageLocation(450, 795)),
            new Location("Cavendish", new ImageLocation(400, 695)),
        };
    }
}