using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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

            builder.Services.AddSingleton(_ => new Configuration {ApiKey = apiKey, UrlRoot = urlRoot});
            builder.Services.AddTransient<IUrlHelper, UrlHelper>();
            builder.Services.AddTransient<ILocationRepository, LocationRepository>();
            builder.Services.AddTransient<ILocationFinder, LocationFinder>();
        }
    }
}
