using System.Net;

namespace NZWalks.API.Controllers
{
    public class Responses
    {
        public HttpStatusCode statusCode { get; set; }
        public string? Message { get; set; }
        public object? Result { get; set; }
    }
}
