using System.Net;

namespace WebApp.Models.Response;

public class ErrorResponse
{
    public HttpStatusCode Status { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
}