using System.Net;

namespace WebApp.Models.Response;

public class ValidationErrorResponse : ErrorResponse
{
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}