using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Enthro.Web
{
    public class Program
    {
        public static async Task Main(String[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder
                .CreateDefault(args);

            builder
                .RootComponents
                .Add<App>("app");

            builder
                .Logging
                .AddConfiguration(builder.Configuration.GetSection("Logging"));

            new Startup(builder.Configuration, builder.HostEnvironment)
                .ConfigureServices(builder.Services);                

            await builder
                .Build()
                .RunAsync();
        }
    }
}