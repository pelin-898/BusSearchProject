using BusSearch.Application.Constants;
using BusSearch.Application.Interfaces;
using BusSearch.Application.Models.Dtos;
using BusSearch.WebUI.ViewModels.Journey;
using Microsoft.AspNetCore.Mvc;

namespace BusSearch.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJourneySearchService _journeySearchService;
        private readonly ILogger<IJourneySearchService> _logger;

        public HomeController(IJourneySearchService journeySearchService, ILogger<IJourneySearchService> logger)
        {
            _journeySearchService = journeySearchService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dto = await _journeySearchService.PrepareDefaultSearchModelAsync();
            var viewModel = new JourneySearchViewModel
            {
                OriginId = dto.OriginId,
                DestinationId = dto.DestinationId,
                OriginName = dto.OriginName,
                DestinationName = dto.DestinationName,
                OriginCityName = dto.OriginCityName,
                DestinationCityName = dto.DestinationCityName,
                DepartureDate = dto.DepartureDate,
                Locations = dto.Locations
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Search(MainPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = ErrorMessages.FillTheForm;

                return RedirectToAction(nameof(Index));
            }
          
            var dto = new JourneySearchDto
            {
                OriginId= model.OriginId,
                DestinationId=model.DestinationId,
                OriginName = model.OriginName,
                DestinationName = model.DestinationName,
                DepartureDate = model.DepartureDate,
            };

            var validationError = _journeySearchService.ValidateSearchModel(dto);

            if (!string.IsNullOrWhiteSpace(validationError))
            {
                TempData["ErrorMessage"] = validationError;
                return RedirectToAction(nameof(Index));
            }

            var routeValues = new
            {
                OriginId = model.OriginId,
                DestinationId = model.DestinationId,
                DepartureDate = model.DepartureDate.ToString("yyyy-MM-dd"),
                OriginName = model.OriginName,
                DestinationName = model.DestinationName
            };

            return RedirectToAction("Index", "Journey", routeValues);
        }

        [HttpGet]
        public async Task<IActionResult> SearchLocations(string keyword)
        {
            var locations = await _journeySearchService.SearchLocationsAsync(keyword);
            return Json(locations);
        }
    }
}
