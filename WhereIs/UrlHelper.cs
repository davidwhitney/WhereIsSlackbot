using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace WhereIs
{
    public class UrlHelper
    {
        private readonly IConfigurationRoot _config;

        public UrlHelper(ExecutionContext context)
        {
            if (context == null)
            {
               // throw new Exception("ExecutionContext is null");
            }

            _config = new ConfigurationBuilder()
                //.SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
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