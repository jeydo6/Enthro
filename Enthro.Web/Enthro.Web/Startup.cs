using AutoMapper;
using Enthro.Application.Repositories;
using Enthro.Domain.Repositories;
using Enthro.Domain.Services;
using Enthro.Domain.Storages;
using Enthro.Infrastructure.Configs;
using Enthro.Infrastructure.Services;
using Enthro.Infrastructure.Storages;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Text.Json;

namespace Enthro.Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        private IWebAssemblyHostEnvironment HostEnvironment { get; }

        public Startup(IConfiguration configuration, IWebAssemblyHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthorizationCore();

            services
                .AddMediatR(typeof(Application.AssemblyMarker))
                .AddAutoMapper(typeof(Application.AssemblyMarker));

            services
                .AddSingleton(
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)
                );

            services
                .AddSingleton(
                    Configuration
                        .GetSection("Endpoints:Enthro.WebAPI")
                        .Get<EndpointConfig>()
                );

            services
                .AddHttpClient("Enthro.WebAPI", (sp, httpClient) =>
                {
                    var endpointConfig = sp.GetRequiredService<EndpointConfig>();

                    httpClient.BaseAddress = new Uri(endpointConfig.Address);
                });
            services
                .AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Enthro.WebAPI"))
                .AddTransient<IAuthenticationService, AuthenticationService>()
                .AddTransient<IIndicatorsService, IndicatorsService>();

            services
                .AddSingleton<ILocalStorage, LocalStorage>()
                .AddSingleton<IIndicatorsRepository, IndicatorsRepository>();

            services
                .AddSingleton<AuthenticationStateProvider, Infrastructure.Providers.AuthenticationStateProvider>();
        }
    }
}