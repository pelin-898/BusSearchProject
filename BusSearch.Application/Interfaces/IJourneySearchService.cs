using BusSearch.Application.Models.Dtos;


namespace BusSearch.Application.Interfaces
{
    public interface IJourneySearchService
    {
        Task<JourneySearchDto> PrepareDefaultSearchModelAsync();
        Task<IEnumerable<LocationSummaryDto>> SearchLocationsAsync(string keyword);
        Task<List<JourneyDto>> OrderJourneyModelAsync( int originId,  int destinationId, string departureDate);
    }
}
