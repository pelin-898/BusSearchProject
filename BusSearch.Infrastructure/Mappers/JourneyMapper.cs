using BusSearch.Application.Models.Dtos;
using BusSearch.Infrastructure.Models.ObiletApi.Journey;

namespace BusSearch.Infrastructure.Mappers
{
    public static class JourneyMapper
    {
        public static JourneyDto ToDto(JourneyResponse model)
        {
            return new JourneyDto
            {
                OriginLocation = model.OriginLocation,
                DestinationLocation = model.DestinationLocation,
                Journey = new JourneyDetailDto
                {
                    Origin = model.Journey.Origin,
                    Destination = model.Journey.Destination,
                    Departure = model.Journey.Departure,
                    Arrival = model.Journey.Arrival,
                    InternetPrice = model.Journey.InternetPrice
                }
            };
        }

        public static List<JourneyDto> ToDtoList(IEnumerable<JourneyResponse> models)
        {
            return models.Select(ToDto).ToList();
        }
    }
}
