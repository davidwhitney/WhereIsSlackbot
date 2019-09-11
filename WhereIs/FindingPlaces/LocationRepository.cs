using System.IO;
using Newtonsoft.Json;

namespace WhereIs.FindingPlaces
{
    public class LocationRepository : ILocationRepository
    {
        public LocationCollection Load(string appRoot)
        {
            var jsonPath = Path.Combine(appRoot, "App_Data", "locations.json");
            var contents = File.ReadAllText(jsonPath);
            return JsonConvert.DeserializeObject<LocationCollection>(contents);
        }
    }
}
