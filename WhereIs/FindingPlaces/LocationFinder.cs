using System;
using System.Linq;

namespace WhereIs.FindingPlaces
{
    public class LocationFinder : ILocationFinder
    {
        private readonly LocationCollection _locations;
        private const int PercentageToleranceForMisspellings = 25;

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

            var maxDistance = (double)nearest.Key.Length / 100 * PercentageToleranceForMisspellings;
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
                Distance = LevenshteinDistance(x.Key, key)
            }).OrderBy(x => x.Distance);

            return new Tuple<Location, int>(
                _locations.Single(x => x.Key == distances.First().Key),
                distances.First().Distance);
        }

        /// <summary>
        /// Who knows how this actually works. Thanks internet.
        /// </summary>
        /// <returns>Distance between two strings</returns>
        public static int LevenshteinDistance(string first, string second)
        {
            var d = new int[first.Length + 1, second.Length + 1];

            if (first.Length == 0) return second.Length;
            if (second.Length == 0) return first.Length;

            for (var i = 0; i <= first.Length; d[i, 0] = i++)
            {
            }

            for (var j = 0; j <= second.Length; d[0, j] = j++)
            {
            }

            for (var i = 1; i <= first.Length; i++)
            {
                for (var j = 1; j <= second.Length; j++)
                {
                    var cost = (second[j - 1] == first[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[first.Length, second.Length];
        }
    }
}