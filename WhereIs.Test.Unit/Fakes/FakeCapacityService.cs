using WhereIs.CapacityMonitoring;
using WhereIs.Infrastructure;

namespace WhereIs.Test.Unit.Fakes
{
    public class FakeCapacityService : ICapacityService
    {
        public int ReturnsThis { get; set; }
        public bool Called { get; private set; }
        public FakeCapacityService(int returnsThis = 0) => ReturnsThis = returnsThis;
        public int NumberOfDesksOccupiedForLocation(LocationFromRequest location) => ReturnsThis;
        public void CheckIn(LocationFromRequest compoundKey) => Called = true;
    }
}