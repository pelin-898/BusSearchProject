using System.Net;

namespace BusSearch.Application.Exceptions
{
    public class ObiletApiException : Exception
    {
        public HttpStatusCode? StatusCode { get; }
        public string? ResponseContent { get; }
        public ObiletApiException()
        {
        }

        public ObiletApiException(string message)
            : base(message)
        {
        }

        public ObiletApiException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public ObiletApiException(string message, HttpStatusCode statusCode, string? responseContent = null, Exception? inner = null)
         : base(message, inner)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }
    }
}
