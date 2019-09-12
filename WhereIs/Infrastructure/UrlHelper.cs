using System;
using System.Web;

namespace WhereIs.Infrastructure
{
    public class UrlHelper : IUrlHelper
    {
        private readonly Configuration _config;

        public UrlHelper(Configuration config) 
            => _config = config ?? throw new ArgumentException("Expected an instance of Configuration to be injected by the runtime.", nameof(config));

        public string ImageFor(string locationKey) 
            => $"{ForUrl("Map")}&key={HttpUtility.UrlEncode(locationKey)}";

        private string ForUrl(string functionName) 
            => $"{_config.UrlRoot}/{functionName}?code={_config.ApiKey}";
    }
}