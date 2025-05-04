using BusSearch.Application.Interfaces;
using BusSearch.Application.ViewModels;
using BusSearch.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BusSearch.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJourneySearchService _journeySearchService;

        public HomeController(IJourneySearchService journeySearchService)
        {
            _journeySearchService = journeySearchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _journeySearchService.PrepareDefaultSearchModelAsync();
            return View(model);
        }

        [HttpPost]
        public IActionResult Search(JourneySearchViewModel model)
        {
            var validationError = _journeySearchService.ValidateSearchModel(model);
            if (!string.IsNullOrEmpty(validationError))
            {
                TempData["ErrorMessage"] = validationError;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index", "Journey", new
            {
                model.OriginId,
                model.DestinationId,
                departureDate = model.DepartureDate.ToString("yyyy-MM-dd"),
                model.OriginName,
                model.DestinationName
            });
        }

        [HttpGet]
        public async Task<IActionResult> SearchLocations(string keyword)
        {
            var locations = await _journeySearchService.SearchLocationsAsync(keyword);
            return Json(locations);
        }
    }
}
