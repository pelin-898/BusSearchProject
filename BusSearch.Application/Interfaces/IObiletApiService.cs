using BusSearch.Application.Models.Dtos;

namespace BusSearch.Application.Interfaces
{
    public interface IObiletApiService
    {
        Task<List<JourneyDto>> GetJourneysAsync(int originId, int destinationId, string departureDate);
        Task<List<BusLocationDto>> GetAllBusLocationsAsync();
        Task<List<BusLocationDto>> SearchBusLocationsAsync(string keyword);
    }
}
