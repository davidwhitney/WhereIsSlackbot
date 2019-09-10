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
            builder.Services.AddTransient(provider =>
            {
                var executionContext = provider.GetService<ExecutionContext>();
                return new ConfigurationBuilder()
                    .SetBasePath(executionContext.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
            });

            builder.Services.AddTransient<UrlHelper>();
            builder.Services.AddTransient<LocationFinder>();
            builder.Services.AddTransient<WhereIsCommand>();
        }
    }
}
