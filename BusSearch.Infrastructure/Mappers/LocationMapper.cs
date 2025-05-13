using BusSearch.Application.Models.Dtos;
using BusSearch.Infrastructure.Models.ObiletApi.Location;

namespace BusSearch.Infrastructure.Mappers
{
    public static class LocationMapper
    {
        public static BusLocationDto ToDto(BusLocationResponse model)
        {
            return new BusLocationDto
            {
                Summary = new LocationSummaryDto
                {
                    Id = model.Id,
                    Name = model.Name,
                    CityName = model.CityName
                },
                ParentId = model.ParentId,
                Rank = model.Rank
            };
        }

        public static List<BusLocationDto> ToDtoList(IEnumerable<BusLocationResponse> models)
        {
            return models.Select(ToDto).ToList();
        }
    }
}
