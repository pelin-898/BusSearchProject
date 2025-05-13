using BusSearch.Application.Constants;
using BusSearch.Application.Exceptions;
using BusSearch.Application.Interfaces;
using BusSearch.Application.Validation;
using BusSearch.WebUI.Mappings;
using BusSearch.WebUI.ViewModels.Journey;
using Microsoft.AspNetCore.Mvc;

namespace BusSearch.WebUI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IJourneySearchService _journeySearchService;
        private readonly IJourneyMappingService _journeyMapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IJourneySearchService journeySearchService, IJourneyMappingService journeyMapper, ILogger<HomeController> logger)
        {
            _journeySearchService = journeySearchService;
            _journeyMapper = journeyMapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var dto = await _journeySearchService.PrepareDefaultSearchModelAsync();
                var viewModel = _journeyMapper.MapToSearchViewModel(dto);
                return View(viewModel);
            }
            catch (ObiletApiException ex)
            {
                _logger.LogError(ex, "Obilet API'den veri al�namad�.");
                SetErrorMessage(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ana sayfa y�klenirken beklenmeyen bir hata olu�tu.");
                SetErrorMessage("Ana sayfa y�klenemedi. L�tfen tekrar deneyin.");
            }

            return View(new JourneySearchViewModel()); 
        }

        [HttpPost]
        public IActionResult Search(JourneySearchBaseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                SetErrorMessage(ErrorMessages.FillTheForm);
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var dto = _journeyMapper.ToDto(model);
                var validationError = JourneySearchValidator.Validate(dto);

                if (!string.IsNullOrWhiteSpace(validationError))
                {
                    SetErrorMessage(validationError);
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToRoute("customJourney", new
                {
                    originId = model.OriginId,
                    destinationId = model.DestinationId,
                    departureDate = model.DepartureDate.ToString("yyyy-MM-dd")
                });
            }
            catch (ObiletApiException ex)
            {
                _logger.LogError(ex, "Obilet API hatas�: Arama s�ras�nda.");
                SetErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Arama s�ras�nda beklenmeyen hata.");
                SetErrorMessage("Arama i�lemi s�ras�nda bir hata olu�tu. L�tfen tekrar deneyin.");
            }

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> SearchLocations(string keyword)
        {
            try
            {
                var locations = await _journeySearchService.SearchLocationsAsync(keyword);
                return Json(locations);
            }
            catch (ObiletApiException ex)
            {
                _logger.LogError(ex, "Lokasyon verisi al�namad�. Keyword: {Keyword}", keyword);
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lokasyon arama s�ras�nda beklenmeyen hata. Keyword: {Keyword}", keyword);
                return Json(new { success = false, message = "Lokasyonlar getirilemedi." });
            }
        }


    }
}
