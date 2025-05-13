namespace BusSearch.Application.Models.Dtos
{
    public class ObiletResponse<T>
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
