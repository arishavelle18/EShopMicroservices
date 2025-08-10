using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    //public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    //{
    //    logger.LogError(
    //        "Error Message: {exceptionMessage}, Time of occurence {time}",
    //        exception.Message, DateTime.UtcNow
    //        );
    //    var problemDetails = new ProblemDetails();
    //    problemDetails.Instance = httpContext.Request.Path;

    //    switch (exception)
    //    {
    //        case ValidationException fluentException:
    //            problemDetails.Title = "one or more validation errors occurred.";
    //            problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
    //            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    //            List<string> validationErrors = fluentException.Errors.Select(error => error.ErrorMessage)
    //                .ToList();
    //            problemDetails.Extensions.Add("errors", validationErrors);
    //            break;
    //        case NotFoundException notFoundException:
    //            problemDetails.Title = notFoundException.Message;
    //            problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
    //            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
    //            problemDetails.Extensions.Add("error", notFoundException.Message);
    //            break;
    //        default:
    //            problemDetails.Title = "An unexpected error occurred.";
    //            problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
    //            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
    //            break;
    //    }

    //    logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);
    //    problemDetails.Status = httpContext.Response.StatusCode;
    //    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
    //    return true;
    //}

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(
            "Error Message: {exceptionMessage}, Time of occurence {time}",
            exception.Message, DateTime.UtcNow
            );

        (string Detail, string Title, int StatusCode,string Type) details = exception switch
        {
            InternalServerException =>
            (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError,
                "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            ),
            ValidationException =>
            (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest,
                "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            ),
            BadRequestException =>
            (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest,
                "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            ),
            NotFoundException =>
            (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound,
                "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4"
            ),
            _ =>
            (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError,
                "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            )
        };

        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Status = details.StatusCode,
            Detail = details.Detail,
            Instance = httpContext.Request.Path,
            Type =  details.Type
        };

        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

        if(exception is ValidationException validation)
        {
            problemDetails.Extensions.Add("errors", validation.Errors.Select(a => a.ErrorMessage));
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        return true;
    }
}
