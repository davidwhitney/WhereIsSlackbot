using System.Collections.Generic;

namespace WhereIs.CapacityMonitoring
{
    public interface ICapacityRepository
    {
        Dictionary<string, int> Load();
        void Save(Dictionary<string, int> state);
    }
}