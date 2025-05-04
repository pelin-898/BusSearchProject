using BusSearch.Application.Constants;
using BusSearch.Application.Interfaces;
using BusSearch.Application.ViewModels.Journey;
using BusSearch.WebUI.ViewModels.Journey;
using Microsoft.AspNetCore.Mvc;

namespace BusSearch.WebUI.Controllers
{
    public class JourneyController : Controller
    {
        private readonly IObiletApiService _obiletService;

        public JourneyController(IObiletApiService obiletService)
        {
            _obiletService = obiletService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int originId, int destinationId, string departureDate, string originName, string destinationName)
        {
            if (!DateTime.TryParse(departureDate, out var parsedDate))
            {
                TempData["ErrorMessage"] = ErrorMessages.ValidDate;
                return RedirectToAction("Index", "Home");
            }

            var journeyItems = await _obiletService.GetJourneysAsync(originId, destinationId, departureDate);

            var journeys = journeyItems.Select(j => new JourneyViewModel
            {
                OriginTerminal = j.OriginTerminal,
                DestinationTerminal = j.DestinationTerminal,
                DepartureTime = j.Departure.ToString("HH:mm"),
                ArrivalTime = j.Arrival.ToString("HH:mm"),
                Price = j.Price
            }).ToList();

            var model = new JourneyIndexViewModel
            {
                OriginName = originName ?? string.Empty,
                DestinationName = destinationName ?? string.Empty,
                DepartureDate = parsedDate.ToString("yyyy-MM-dd"),
                Journeys = journeys
            };

            return View(model);
        }
    }
}
