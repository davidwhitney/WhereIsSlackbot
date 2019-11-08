namespace WhereIs.Infrastructure
{
    public class LocationFromRequest
    {
        public string Value { get; }

        public LocationFromRequest(string location)
        {
            location = location ?? "";
            Value = location.ToLower().Trim();
        }

        public static implicit operator string(LocationFromRequest instance) => instance.Value;
        public static implicit operator LocationFromRequest(string key) => new LocationFromRequest(key);

        public bool IsValid() => !string.IsNullOrWhiteSpace(Value);
    }
}