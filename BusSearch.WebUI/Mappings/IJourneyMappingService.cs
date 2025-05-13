using BusSearch.Application.Models.Dtos;
using BusSearch.Application.ViewModels.Journey;
using BusSearch.WebUI.ViewModels.Journey;

namespace BusSearch.WebUI.Mappings
{
    public interface IJourneyMappingService
    {
        List<JourneyViewModel> MapToViewModel(List<JourneyDto> dtos);
        JourneySearchViewModel MapToSearchViewModel(JourneySearchDto dto);
        JourneySearchDto ToDto(JourneySearchBaseViewModel model);
    }
}
