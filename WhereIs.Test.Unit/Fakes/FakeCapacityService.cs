using WhereIs.CapacityMonitoring;

namespace WhereIs.Test.Unit.Fakes
{
    public class FakeCapacityService : ICapacityService
    {
        public int ReturnsThis { get; set; }
        public FakeCapacityService(int returnsThis = 0) => ReturnsThis = returnsThis;
        public int NumberOfDesksOccupiedForLocation(string location) => ReturnsThis;
        public void CheckIn(string compoundKey)
        {
            
        }
    }
}