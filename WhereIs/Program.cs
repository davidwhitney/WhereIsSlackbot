using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WhereIs
{
    public class Program
    {
        public static void Main(string[] args) => BuildWebHost(args).Run();
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) 
            => services.AddTransient<WhereIsMiddleware>();
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) 
            => app.UseMiddleware<WhereIsMiddleware>();
    }
}
