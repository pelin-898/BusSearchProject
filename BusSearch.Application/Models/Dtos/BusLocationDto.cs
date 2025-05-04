namespace BusSearch.Application.Models.Dtos
{
	public class BusLocationDto
	{
		public int Id { get; set; }
		public int? ParentId { get; set; }
		public string CityName { get; set; }
		public string Name { get; set; }
		public int? Rank { get; set; }

	}
}
