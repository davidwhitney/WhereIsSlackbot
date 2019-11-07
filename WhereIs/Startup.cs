using System;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using WhereIs.CapacityMonitoring;
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
            var config = new Configuration
            {
                ApiKey = Environment.GetEnvironmentVariable("ApiKey"),
                UrlRoot = Environment.GetEnvironmentVariable("UrlRoot"),
                Root = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot") ??
                       $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot",
                BlobCredentials = Environment.GetEnvironmentVariable("BlobCredentials")
            };

            var cache = new MemoryCache(new MemoryCacheOptions());

            builder.Services.AddSingleton(_ => config);
            builder.Services.AddSingleton<IMemoryCache>(_ => cache);
            builder.Services.AddTransient<IUrlHelper, UrlHelper>();
            builder.Services.AddTransient<ILocationRepository>(_ => new LocationRepository(config.Root));
            builder.Services.AddTransient(_ => _.GetService<ILocationRepository>().Load());
            builder.Services.AddTransient<ILocationFinder, LocationFinder>();
            builder.Services.AddTransient<IImageGenerator, ImageGenerator>();
            builder.Services.AddTransient<ICapacityService, CapacityService>();
            builder.Services.AddTransient<CapacityRepository, CapacityRepository>();
        }
    }
}
