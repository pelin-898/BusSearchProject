using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace BusSearch.Application.Models.Dtos
{
	public class JourneyItemDto
	{
		public string OriginLocation { get; set; }
		public string DestinationLocation { get; set; }
		public string OriginTerminal { get; set; }
		public string DestinationTerminal { get; set; }
		public DateTime Departure { get; set; }
		public DateTime Arrival { get; set; }
		public decimal Price { get; set; }
	}

}
