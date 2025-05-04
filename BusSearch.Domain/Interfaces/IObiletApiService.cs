using BusSearch.Domain.Journey;
using BusSearch.Domain.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSearch.Domain.Interfaces
{
    public interface IObiletApiService
    {
        Task EnsureSessionAsync();
        Task<List<JourneyItem>> GetJourneysAsync(int originId, int destinationId, string departureDate);
        Task<List<BusLocation>> GetAllBusLocationsAsync();
        Task<List<BusLocation>> SearchBusLocationsAsync(string keyword);


    }
}
