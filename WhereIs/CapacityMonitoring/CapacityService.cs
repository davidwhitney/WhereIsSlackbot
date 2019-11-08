using System.Linq;
using WhereIs.Infrastructure;

namespace WhereIs.CapacityMonitoring
{
    public class CapacityService : ICapacityService
    {
        private readonly ICapacityRepository _repo;

        public CapacityService(ICapacityRepository repo)
        {
            _repo = repo;
        }

        public int NumberOfDesksOccupiedForLocation(LocationFromRequest location)
        {
            var state = _repo.Load();
            var keysInThisRegion = state.Keys.Where(x => x.ToLower().StartsWith(location.Value));
            return keysInThisRegion.Sum(key => state[key]);
        }

        public void CheckIn(LocationFromRequest compoundKey)
        {
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