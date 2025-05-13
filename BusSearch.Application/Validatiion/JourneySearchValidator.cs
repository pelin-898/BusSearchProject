// Dosya: BusSearch.Application/Validation/JourneySearchValidator.cs

using BusSearch.Application.Constants;
using BusSearch.Application.Models.Dtos;

namespace BusSearch.Application.Validation
{
    public static class JourneySearchValidator
    {
        public static string? Validate(JourneySearchDto model)
        {
            if (model.Origin.Id == 0 || model.Destination.Id == 0 )
                return ErrorMessages.FillTheForm;

            if (model.Origin.Id == model.Destination.Id)
                return ErrorMessages.SameLocation;

            if (model.DepartureDate < DateTime.Now.Date)
                return ErrorMessages.PastDate;

            return null;
        }
    }
}
