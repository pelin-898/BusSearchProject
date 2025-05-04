using BusSearch.Application.Models.Dtos;


namespace BusSearch.Application.Interfaces
{
    public interface IJourneySearchService
    {
        Task<JourneySearchDto> PrepareDefaultSearchModelAsync();
        string ValidateSearchModel(JourneySearchDto model);
        Task<IEnumerable<LocationSearchDto>> SearchLocationsAsync(string keyword);
    }
}
