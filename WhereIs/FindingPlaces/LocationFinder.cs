using System;
using System.Linq;

namespace WhereIs.FindingPlaces
{
    public class LocationFinder : ILocationFinder
    {
        private readonly LocationCollection _locations;
        private const int PercentageToleranceForMisspellings = 35;

        public LocationFinder(LocationCollection locations)
        {
            _locations = locations;
        }

        public Location Find(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return Location.NotFound;
            }

            var key = location.ToLower();
            var exactMatch = _locations.SingleOrDefault(x => x.Key == key);
            if (exactMatch != null)
            {
                return exactMatch;  
            }

            var (nearest, distance) = ReturnNearestSpellingMatch(key);

            var maxDistance = (double)nearest.Name.Length / 100 * PercentageToleranceForMisspellings;
            var roundedDistance = Math.Round(maxDistance, MidpointRounding.AwayFromZero);

            return distance <= roundedDistance
                ? nearest
                : Location.NotFound;
        }

        private Tuple<Location, int> ReturnNearestSpellingMatch(string key)
        {
            var distances = _locations.Select(x => new
            {
                x.Key,
                Distance = LevenshteinDistance.Compute(x.Key, key)
            }).OrderBy(x => x.Distance);

            return new Tuple<Location, int>(
                _locations.Single(x => x.Key == distances.First().Key),
                distances.First().Distance);
        }
    }
}