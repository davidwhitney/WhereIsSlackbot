namespace WhereIs.FindingPlaces
{
    public interface ILocationRepository
    {
        LocationCollection Load(string appRoot);
    }
}