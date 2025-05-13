using BusSearch.Application.Models.Dtos;
using BusSearch.Application.ViewModels.Journey;
using BusSearch.WebUI.ViewModels.Journey;

namespace BusSearch.WebUI.Mappings
{
    public class JourneyMappingService : IJourneyMappingService
    {
        public List<JourneyViewModel> MapToViewModel(List<JourneyDto> dtos)
        {
            return dtos.Select(j => new JourneyViewModel
            {
                Origin = j.Journey.Origin,
                Destination = j.Journey.Destination,
                DepartureTime = j.Journey.Departure.ToString("HH:mm"),
                ArrivalTime = j.Journey.Arrival.ToString("HH:mm"),
                InternetPrice = j.Journey.InternetPrice
            }).ToList();
        }

        public JourneySearchViewModel MapToSearchViewModel(JourneySearchDto dto) => new()
        {
            OriginId = dto.Origin.Id,
            DestinationId = dto.Destination.Id,
            OriginName = dto.Origin.Name,
            DestinationName = dto.Destination.Name,
            OriginCityName = dto.Origin.CityName,
            DestinationCityName = dto.Destination.CityName,
            DepartureDate = dto.DepartureDate
        };

        public JourneySearchDto ToDto(JourneySearchBaseViewModel model) => new()
        {
            Origin = new LocationSummaryDto
            {
                Id = model.OriginId,
                Name = model.OriginName
            },
            Destination = new LocationSummaryDto
            {
                Id = model.DestinationId,
                Name = model.DestinationName
            },
            DepartureDate = model.DepartureDate
        };
    }
}
