using WhereIs.FindingPlaces;

namespace WhereIs.Test.Unit.Fakes
{
    public class FakeLocationRepository : ILocationRepository
    {
        private readonly LocationCollection _returnThese;
        public FakeLocationRepository(LocationCollection returnThese) => _returnThese = returnThese;
        public LocationCollection Load(string appRoot) => _returnThese;
    }
}