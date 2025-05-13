using BusSearch.Application.Constants;
using BusSearch.Application.Exceptions;
using BusSearch.Application.Interfaces;
using BusSearch.Application.ViewModels.Journey;
using BusSearch.WebUI.Mappings;
using BusSearch.WebUI.ViewModels.Journey;
using Microsoft.AspNetCore.Mvc;

namespace BusSearch.WebUI.Controllers
{
    public class JourneyController : BaseController
    {
        private readonly IJourneySearchService _journeySearchService;
        private readonly IJourneyMappingService _journeyMapper;
        private readonly ILogger<JourneyController> _logger;

        public JourneyController(IJourneyMappingService journeyMapper, IJourneySearchService journeySearchService, ILogger<JourneyController> logger)
        {
            _journeyMapper = journeyMapper;
            _journeySearchService = journeySearchService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int originId, int destinationId, string departureDate)
        {
            if (!DateTime.TryParse(departureDate, out var parsedDate))
            {
                SetErrorMessage(ErrorMessages.ValidDate);
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var journeyItems = await _journeySearchService.OrderJourneyModelAsync(originId, destinationId, departureDate);
                var journeys = _journeyMapper.MapToViewModel(journeyItems);
                var viewModel = BuildViewModel(parsedDate, journeys);

                return View(viewModel);
            }
            catch (ObiletApiException ex)
            {
                _logger.LogError(ex, "Obilet API hatası: Sefer bilgileri alınamadı.");
                SetErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Seferler alınırken beklenmeyen bir hata oluştu. OriginId: {OriginId}, DestinationId: {DestinationId}, DepartureDate: {Date}", originId, destinationId, departureDate);
                SetErrorMessage("Seferler getirilemedi. Lütfen tekrar deneyin.\"");
            }

            return RedirectToAction("Index", "Home");
        }


        private JourneyIndexViewModel BuildViewModel(DateTime parsedDate, List<JourneyViewModel> journeys)
        {
            return new JourneyIndexViewModel
            {
                DepartureDate = parsedDate.ToString("yyyy-MM-dd"),
                Journeys = journeys
            };
        }
    }
}
