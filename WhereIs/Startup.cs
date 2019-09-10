using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using WhereIs.Slack;

[assembly: FunctionsStartup(typeof(WhereIs.Startup))]

namespace WhereIs
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<UrlHelper, UrlHelper>();
            builder.Services.AddTransient<LocationFinder, LocationFinder>();
        }
    }
}
