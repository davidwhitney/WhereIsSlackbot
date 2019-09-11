using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using WhereIs.FindingPlaces;
using WhereIs.Infrastructure;

[assembly: FunctionsStartup(typeof(WhereIs.Startup))]

namespace WhereIs
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var urlRoot = Environment.GetEnvironmentVariable("UrlRoot");
            var apiKey = Environment.GetEnvironmentVariable("ApiKey");

            var localRoot = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot");
            var azureRoot = $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot";
            var actualRoot = localRoot ?? azureRoot;

            builder.Services.AddSingleton(_ => new Configuration {ApiKey = apiKey, UrlRoot = urlRoot});
            builder.Services.AddTransient<IUrlHelper, UrlHelper>();
            builder.Services.AddSingleton<ILocationRepository>(_ => new LocationRepository(actualRoot));
            builder.Services.AddSingleton(_ => _.GetService<ILocationRepository>().Load());
            builder.Services.AddTransient<ILocationFinder, LocationFinder>();
        }
    }
}
