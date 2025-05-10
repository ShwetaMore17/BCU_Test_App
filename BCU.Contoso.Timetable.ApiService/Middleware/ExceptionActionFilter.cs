using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Polly.CircuitBreaker;
using Polly.Timeout;
using Refit;
using System.Net;
using System.Text.Json;

namespace BCU.Contoso.Timetable.ApiService.Middleware;

public class ExceptionActionFilter(ILogger<ExceptionActionFilter> logger) : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        ObjectResult MakeResponse(HttpStatusCode code, string? message = null) =>
            new
                (
                new Refit.ProblemDetails
                {
                    Detail = message,
                    Title = code.ToString(),
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                    Status = (int)code
                }
            )
            { StatusCode = (int)code };

        if (context.Exception is ValidationApiException validationApiException)
        {
            context.Result = new BadRequestObjectResult(validationApiException.Content);

            logger.LogError(context.Exception, JsonSerializer.Serialize(validationApiException.Content));

            context.ExceptionHandled = true;
        }
        else if (context.Exception is ApiException apiException)
        {
            logger.LogError(context.Exception, apiException.Content ?? "NO CONTENT");

            if (string.IsNullOrWhiteSpace(apiException.Content) || apiException.StatusCode == HttpStatusCode.InternalServerError)
            {
                context.Result = MakeResponse(apiException.StatusCode);
            }
            else
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)apiException.StatusCode,
                    Content = apiException.Content,
                    ContentType = "application/json"
                };
            }

            context.ExceptionHandled = true;
        }
    }
}