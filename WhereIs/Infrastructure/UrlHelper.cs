using System;
using Microsoft.Extensions.Configuration;
using WhereIs.Commands;

namespace WhereIs.Infrastructure
{
    public interface IUrlHelper
    {
        string ImageFor(string locationKey);
    }

    public class UrlHelper : IUrlHelper
    {
        private readonly IConfiguration _config;

        public UrlHelper(IConfiguration config)
        {
            _config = config ?? throw new Exception("Expected an instance of IConfiguration to be injected by the runtime.");
        }

        public string ForUrl(string functionName)
        {
            var apiKey = _config["Values:ApiKey"];
            var apiRoot = _config["Values:UrlRoot"];
            return $"{apiRoot}/{functionName}?code={apiKey}";
        }

        public string ImageFor(string locationKey)
        {
            var imageUrl = ForUrl(nameof(MapCommand.Map));
            return $"https://whereis.azurewebsites.net/api/Map?code=GaE17Cl8iaGFzbZN663XpI6/5L5lda8ANxeQi4YaZkwTawiTLwKISA==&key={locationKey}";
        }
    }
}