using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace WhereIs.Test.Unit.Fakes
{
    public class FakeConfiguration : Dictionary<string, string>, IConfiguration
    {
        public FakeConfiguration()
        {
            Add("Values:ApiKey", "key123");
            Add("Values:UrlRoot", "https://localhost/api");
        }

        public IConfigurationSection GetSection(string key) => throw new NotImplementedException();

        public IEnumerable<IConfigurationSection> GetChildren() => throw new NotImplementedException();

        public IChangeToken GetReloadToken() => throw new NotImplementedException();
    }
}