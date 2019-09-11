using System.IO;
using Newtonsoft.Json;

namespace WhereIs.FindingPlaces
{
    public class LocationRepository : ILocationRepository
    {
        private readonly string _appRoot;
        public LocationRepository(string appRoot) => _appRoot = appRoot;

        public LocationCollection Load()
        {
            var jsonPath = Path.Combine(_appRoot, "App_Data", "locations.json");
            var contents = File.ReadAllText(jsonPath);
            return JsonConvert.DeserializeObject<LocationCollection>(contents);
        }
    }
}
