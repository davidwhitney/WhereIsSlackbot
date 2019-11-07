using System.Linq;

namespace WhereIs.CapacityMonitoring
{
    public class CapacityService : ICapacityService
    {
        private readonly CapacityRepository _repo;

        public CapacityService(CapacityRepository repo)
        {
            _repo = repo;
        }

        public int NumberOfDesksOccupiedForLocation(string location)
        {
            var state = _repo.Load();
            var keysInThisRegion = state.Keys.Where(x => x.ToLower().StartsWith(location.ToLower()));
            return keysInThisRegion.Sum(key => state[key]);
        }

        public void CheckIn(string compoundKey)
        {
            compoundKey = compoundKey.ToLower().Trim();
            var state = _repo.Load();
            if (!state.ContainsKey(compoundKey))
            {
                state.Add(compoundKey, 0);
            }

            state[compoundKey]++;

            _repo.Save(state);
        }

    }
}