using BusSearch.Application.Interfaces;
using BusSearch.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using BusSearch.Infrastructure.Configurations;


namespace BusSearch.Infrastructure.ServiceRegistration
{
    public static class BusApiServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
			// ClientDefaults binding
			services.Configure<ClientDefaults>(configuration.GetSection("ClientDefaults"));

			// External Api HttpClient setup
			services.AddHttpClient<IObiletApiService, ObiletApiService>(client =>
            {
                client.BaseAddress = new Uri(configuration["ObiletApi:BaseUrl"]);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", configuration["ObiletApi:Token"]);
            });

            return services;
        }
    }
}
