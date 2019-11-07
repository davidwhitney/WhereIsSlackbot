namespace WhereIs.CapacityMonitoring
{
    public interface ICapacityService
    {
        int NumberOfDesksOccupiedForLocation(string location);
        void CheckIn(string compoundKey);
        void Reset();
    }
}