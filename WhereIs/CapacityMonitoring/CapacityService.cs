using System;
using System.Collections.Generic;
using System.Linq;

namespace WhereIs.CapacityMonitoring
{
    public class CapacityService : ICapacityService
    {
        private Dictionary<string, int> _ledger;
        public CapacityService() => _ledger = new Dictionary<string, int>();

        public int NumberOfDesksOccupiedForLocation(string location)
        {
            return _ledger[location];
        }

        public void CheckIn(string compoundKey)
        {
            var locationKey = compoundKey.ToLower().Split("::").FirstOrDefault();
            if (!compoundKey.Contains("::") || locationKey == null)
            {
                throw new Exception("Invalid key for checkin.");
            }

            if (!_ledger.ContainsKey(locationKey))
            {
                _ledger.Add(locationKey, 0);
            }

            _ledger[locationKey]++;
        }
    }
}