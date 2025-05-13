using BusSearch.Application.Constants;
using BusSearch.Application.Interfaces;
using BusSearch.Application.Models.Dtos;
using Microsoft.Extensions.Logging;

namespace BusSearch.Application.Services
{
    public class JourneySearchService : IJourneySearchService
    {
        private readonly IObiletApiService _apiService;
        private readonly ILogger<JourneySearchService> _logger;

        public JourneySearchService(IObiletApiService apiService, ILogger<JourneySearchService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

  
        public async Task<JourneySearchDto> PrepareDefaultSearchModelAsync()
        {
            var locations = await _apiService.GetAllBusLocationsAsync();
            var sortedLocations = locations.OrderBy(x => x.Rank ?? int.MaxValue).ToList();

            var defaultOrigin = sortedLocations.FirstOrDefault();
            var defaultDestination = sortedLocations.ElementAtOrDefault(2); // 3. şehir varış olarak alındı

            return CreateSearchDto(defaultOrigin, defaultDestination, sortedLocations);
        }

        public async Task<List<JourneyDto>> OrderJourneyModelAsync(int originId, int destinationId, string departureDate)
        {
            var journeys = await _apiService.GetJourneysAsync(originId, destinationId, departureDate);
            return journeys.OrderBy(x => x.Journey.Departure).ToList();
        }

        public async Task<IEnumerable<LocationSummaryDto>> SearchLocationsAsync(string keyword)
        {
            var locations = string.IsNullOrWhiteSpace(keyword)
                ? await _apiService.GetAllBusLocationsAsync()
                : await _apiService.SearchBusLocationsAsync(keyword);

            return locations.Select(l => new LocationSummaryDto
            {
                Id = l.Summary.Id,
                Name = l.Summary.Name,
                CityName = l.Summary.CityName ?? string.Empty
            });
        }


        private JourneySearchDto CreateSearchDto(BusLocationDto? defaultOrigin, BusLocationDto? defaultDestination, List<BusLocationDto> locations)
        {
            return new JourneySearchDto
            {
                Origin = defaultOrigin?.Summary,
                Destination = defaultDestination?.Summary,
                DepartureDate = DateTime.Today.AddDays(1),
                Locations = locations
            };
        }
    }
}
