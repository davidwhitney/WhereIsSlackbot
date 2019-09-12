using System;

namespace WhereIs.FindingPlaces
{
    /// <summary>
    /// Who knows how this actually works. Thanks internet.
    /// </summary>
    public static class LevenshteinDistance
    {
        public static int Compute(string first, string second)
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