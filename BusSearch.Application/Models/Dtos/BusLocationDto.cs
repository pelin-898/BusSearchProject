namespace BusSearch.Application.Models.Dtos
{
	public class BusLocationDto 
	{
        public int? ParentId { get; set; }

        public int? Rank { get; set; }

        public LocationSummaryDto Summary { get; set; }
    }
}
