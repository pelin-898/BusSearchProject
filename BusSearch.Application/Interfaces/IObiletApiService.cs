using BusSearch.Application.Models.Dtos;

namespace BusSearch.Application.Interfaces
{
    public interface IObiletApiService
    {
        Task EnsureSessionAsync();
        Task<List<JourneyItemDto>> GetJourneysAsync(int originId, int destinationId, string departureDate);
        Task<List<BusLocationDto>> GetAllBusLocationsAsync();
        Task<List<BusLocationDto>> SearchBusLocationsAsync(string keyword);
    }
}
