using System;
using System.Collections.Generic;
namespace BusSearch.Application.Models.Dtos
{
    public class LocationSearchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
    }
}
