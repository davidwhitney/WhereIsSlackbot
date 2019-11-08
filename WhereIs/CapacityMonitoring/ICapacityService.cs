using WhereIs.Infrastructure;

namespace WhereIs.CapacityMonitoring
{
    public interface ICapacityService
    {
        int NumberOfDesksOccupiedForLocation(LocationFromRequest location);
        void CheckIn(LocationFromRequest compoundKey);
    }
}