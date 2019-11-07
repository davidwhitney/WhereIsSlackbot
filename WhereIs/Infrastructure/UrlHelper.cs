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
            => $"{_config.UrlRoot}/Map?code={_config.ApiKey}&key={HttpUtility.UrlEncode(locationKey)}";

        public string CapacityImageFor(string locationKey) 
            => $"{_config.UrlRoot}/HeatMap?code={_config.CapacityApiKey}&key={HttpUtility.UrlEncode(locationKey)}";
    }
}