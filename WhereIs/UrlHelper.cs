using System;
using Microsoft.Extensions.Configuration;

namespace WhereIs
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
            return $"{imageUrl}&{locationKey}";
        }
    }
}