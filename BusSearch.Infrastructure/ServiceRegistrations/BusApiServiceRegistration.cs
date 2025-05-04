using BusSearch.Application.Interfaces;
using BusSearch.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using BusSearch.Infrastructure.Configurations;
using BusSearch.Application.Services;
using BusSearch.Domain.Interfaces;

namespace BusSearch.Infrastructure.ServiceRegistration
{
    public static class BusApiServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // IOptions<ClientDefaults> binding
            services.Configure<ClientDefaults>(configuration.GetSection("ClientDefaults"));

            services.AddHttpClient<IObiletApiService, ObiletApiService>(client =>
            {
                client.BaseAddress = new Uri(configuration["ObiletApi:BaseUrl"]);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", configuration["ObiletApi:Token"]);
            });

            services.AddScoped<IJourneySearchService, JourneySearchService>();

            services.Configure<ClientDefaults>(configuration.GetSection("ClientDefaults"));

            return services;
        }
    }
}
