using BusSearch.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSearch.Application.Interfaces
{
    public interface IJourneySearchService
    {
        Task<JourneySearchViewModel> PrepareDefaultSearchModelAsync();
        string ValidateSearchModel(JourneySearchViewModel model);
        Task<IEnumerable<object>> SearchLocationsAsync(string keyword);
    }
}
