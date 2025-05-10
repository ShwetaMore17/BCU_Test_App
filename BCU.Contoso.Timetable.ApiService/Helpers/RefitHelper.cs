using Refit;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;

namespace BCU.Contoso.Timetable.ApiService.Helpers;

public interface IRefitHelper
{
    ApiException CreateException(HttpStatusCode statusCode, string message);
}

[ExcludeFromCodeCoverage]
public class RefitHelper : IRefitHelper
{
    public ApiException CreateException(HttpStatusCode statusCode, string message)
    {
        return ApiException.Create
            (
                new HttpRequestMessage(HttpMethod.Get, new Uri("https://bcu.ac.uk")),
                HttpMethod.Get,
                new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent
                    (
                        JsonSerializer.Serialize
                        (
                            new ProblemDetails
                            {
                                Detail = message,
                                Title = statusCode.ToString(),
                                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                                Status = (int)statusCode
                            }
                        )
                    )
                },
                new RefitSettings()
            )
            .Result;
    }
}
