using System;
using System.Collections.Generic;
using System.Linq;

namespace WhereIs.FindingPlaces
{
    public interface ILocationFinder
    {
        Location Find(string location);
    }

    public class LocationFinder : ILocationFinder
    {
        private readonly List<Location> _locations;
        private readonly int _percentageOfSpellingMistakeThatMatches = 25;

        public LocationFinder() : this(HardcodedLocations.Items)
        {
        }

        public LocationFinder(IEnumerable<Location> knownLocations)
        {
            _locations = (knownLocations ?? new List<Location>()).ToList();
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
            var percentageDifferent = 100 - (int)Math.Round((double)(100 * (nearest.Key.Length - distance)) / nearest.Key.Length);

            return percentageDifferent < _percentageOfSpellingMistakeThatMatches
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
                _locations.Single(x => x.Key == distances.Last().Key),
                distances.Last().Distance);
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