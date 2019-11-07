namespace WhereIs.CapacityMonitoring
{
    public interface ICapacityService
    {
        int NumberOfDesksOccupiedForLocation(string location);
    }
}