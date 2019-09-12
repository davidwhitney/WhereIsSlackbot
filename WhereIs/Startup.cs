using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using WhereIs.FindingPlaces;
using WhereIs.ImageGeneration;
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

            var config = new Configuration {ApiKey = apiKey, UrlRoot = urlRoot, Root = actualRoot};
            var cache = new MemoryCache(new MemoryCacheOptions
            { 
                SizeLimit = 1024 * 30
            });

            builder.Services.AddSingleton(_ => config);
            builder.Services.AddSingleton<IMemoryCache>(_ => cache);
            builder.Services.AddTransient<IUrlHelper, UrlHelper>();
            builder.Services.AddTransient<ILocationRepository>(_ => new LocationRepository(actualRoot));
            builder.Services.AddTransient(_ => _.GetService<ILocationRepository>().Load());
            builder.Services.AddTransient<ILocationFinder, LocationFinder>();
            builder.Services.AddTransient<IImageGenerator, ImageGenerator>();
        }
    }
}
