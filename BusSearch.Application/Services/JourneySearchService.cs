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
            _logger.LogInformation("Varsayılan arama modeli hazırlanıyor.");
            try
            {
                var locations = await _apiService.GetAllBusLocationsAsync();
                var sortedLocations = locations.OrderBy(x => x.Rank ?? int.MaxValue).ToList();

                var defaultOrigin = sortedLocations.FirstOrDefault();
                var defaultDestination = sortedLocations.ElementAtOrDefault(2);

                return MapToDto(defaultOrigin, defaultDestination, sortedLocations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Varsayılan arama modeli hazırlanırken bir hata oluştu.");
                throw;
            }
        }

        public string ValidateSearchModel(JourneySearchDto model)
        {
            _logger.LogInformation("Arama modeli doğrulanıyor. OriginId: {OriginId}, DestinationId: {DestinationId}, Date: {Date}",
                model.OriginId, model.DestinationId, model.DepartureDate.ToShortDateString());

            if (model.OriginId == model.DestinationId)
            {
                _logger.LogWarning("Origin ve Destination aynı seçildi.");
                return ErrorMessages.SameLocation;
            }

            if (model.DepartureDate.Date < DateTime.Today)
            {
                _logger.LogWarning("Geçmiş tarih seçildi: {Date}", model.DepartureDate.ToShortDateString());
                return ErrorMessages.PastDate;
            }

            return null;
        }

        public async Task<IEnumerable<LocationSearchDto>> SearchLocationsAsync(string keyword)
        {
            _logger.LogInformation("Lokasyon aranıyor. Anahtar kelime: {Keyword}", keyword);
            try
            {
                var locations = string.IsNullOrWhiteSpace(keyword)
                    ? await _apiService.GetAllBusLocationsAsync()
                    : await _apiService.SearchBusLocationsAsync(keyword);

                return locations.Select(l => new LocationSearchDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    ParentName = l.CityName ?? ""
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lokasyon arama işlemi sırasında hata oluştu.");
                throw;
            }
        }

        private JourneySearchDto MapToDto(BusLocationDto defaultOrigin, BusLocationDto defaultDestination, List<BusLocationDto> locations)
        {
            return new JourneySearchDto
            {
                Locations = locations,
                OriginId = defaultOrigin?.Id ?? 0,
                DestinationId = defaultDestination?.Id ?? 0,
                OriginName = defaultOrigin?.Name ?? "",
                DestinationName = defaultDestination?.Name ?? "",
                OriginCityName = defaultOrigin?.CityName ?? "",
                DestinationCityName = defaultDestination?.CityName ?? "",
                DepartureDate = DateTime.Today.AddDays(1)
            };
        }
    }
}
