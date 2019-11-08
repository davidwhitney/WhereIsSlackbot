using System.Collections.Generic;
using System.Linq;

namespace WhereIs.FindingPlaces
{
    /// <summary>
    /// Helps with container injection
    /// </summary>
    public class LocationCollection : List<Location>
    {
        public IEnumerable<Location> SublocationsOf(string name) => this.Where(x => x.Name.StartsWith(name + "::"));
        public int TotalCapacityOf(string name) => SublocationsOf(name).Sum(x => x.Capacity);
    }
}