using BusSearch.Application.Interfaces;
using BusSearch.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BusSearch.Application.ServiceRegistration
{
		public static class ApplicationServiceRegistration
		{
			public static IServiceCollection AddApplication(this IServiceCollection services)
			{
				services.AddScoped<IJourneySearchService, JourneySearchService>();
				return services;
			}
		}
}
