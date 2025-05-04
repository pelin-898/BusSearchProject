using BusSearch.Application.Constants;
using BusSearch.Application.Interfaces;
using BusSearch.Application.ViewModels;
using BusSearch.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<JourneySearchViewModel> PrepareDefaultSearchModelAsync()
        {
            _logger.LogInformation("Varsayılan arama modeli hazırlanıyor.");
            try
            {

                var locations = await _apiService.GetAllBusLocationsAsync();
                var sortedLocations = locations.OrderBy(x => x.Rank ?? int.MaxValue).ToList();

                var defaultOrigin = sortedLocations.FirstOrDefault();
                var defaultDestination = sortedLocations.ElementAtOrDefault(2);

                return new JourneySearchViewModel
                {
                    Locations = sortedLocations ?? new(),
                    OriginId = defaultOrigin?.Id ?? 0,
                    DestinationId = defaultDestination?.Id ?? 0,
                    OriginName = defaultOrigin?.Name ?? "",
                    DestinationName = defaultDestination?.Name ?? "",
                    OriginCityName = defaultOrigin?.CityName ?? "",
                    DestinationCityName = defaultDestination?.CityName ?? "",
                    DepartureDate = DateTime.Today.AddDays(1)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Varsayılan arama modeli hazırlanırken bir hata oluştu.");
                throw;
            }
        }

        public string ValidateSearchModel(JourneySearchViewModel model)
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

        public async Task<IEnumerable<object>> SearchLocationsAsync(string keyword)
        {
            _logger.LogInformation("Lokasyon aranıyor. Anahtar kelime: {Keyword}", keyword);
            try
            {

                var locations = string.IsNullOrWhiteSpace(keyword)
                ? await _apiService.GetAllBusLocationsAsync()
                : await _apiService.SearchBusLocationsAsync(keyword);

                return locations.Select(l => new
                {
                    l.Id,
                    l.Name,
                    ParentName = l.CityName ?? ""
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lokasyon arama işlemi sırasında hata oluştu.");
                throw;
            }

        }
    }
}
