using BusSearch.Application.ViewModels;
using BusSearch.Domain.Interfaces;
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
            if (string.IsNullOrEmpty(departureDate))
                return BadRequest("Tarih bilgisi geçersiz.");

            var journeyData = await _obiletService.GetJourneysAsync(originId, destinationId, departureDate);

            var journeys = journeyData?.Select(j => new JourneyViewModel
            {
                OriginName = j.OriginLocation,
                DestinationName = j.DestinationLocation,
                OriginTerminal = j.Journey.Origin,
                DestinationTerminal = j.Journey.Destination,
                DepartureTime = j.Journey.Departure.ToString("HH:mm"),
                ArrivalTime = j.Journey.Arrival.ToString("HH:mm"),
                Price = j.Journey.InternetPrice
            }).ToList() ?? new List<JourneyViewModel>();

            var model = new JourneyIndexViewModel
            {
                OriginName = originName ?? string.Empty,
                DestinationName = destinationName ?? string.Empty,
                DepartureDate = departureDate,
                Journeys = journeys
            };

            return View(model);
        }
    }
}
