using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SkillMasteryAPI.Application.CrossCutting;

namespace SkillMasteryAPI.Presentation.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {

        var exceptionMessage = exception.Message;

        switch(exception)
        {
            case EmptyIdException:
                {
                    var badRequestProblemDetails = new ProblemDetails
                    {
                        Title = "The specified resource was not found!",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = exceptionMessage,
                        Instance = $"urn:wakuwaku:error:{Guid.NewGuid()}"
                    };

                    httpContext.Response.StatusCode = badRequestProblemDetails.Status.Value;
                    await httpContext.Response.WriteAsJsonAsync(
                        badRequestProblemDetails, cancellationToken);
                    return true;
                }
            case NotFoundException:
                {
                    var notFoundProblemDetails = new ProblemDetails
                    {
                        Title = "The specified resource was not found!",
                        Status = StatusCodes.Status404NotFound,
                        Detail = exceptionMessage
                    };
                    httpContext.Response.StatusCode = notFoundProblemDetails.Status.Value;
                    await httpContext.Response.WriteAsJsonAsync(notFoundProblemDetails, cancellationToken);

                    return true;
                }
            case FoundException:
                {
                    var foundProblemDetails = new ProblemDetails
                    {
                        Title = "The specified resource was found but is used in another resource!",
                        Status = StatusCodes.Status409Conflict,
                        Detail = exceptionMessage
                    };
                    httpContext.Response.StatusCode = foundProblemDetails.Status.Value;
                    await httpContext.Response.WriteAsJsonAsync(foundProblemDetails, cancellationToken);

                    return true;
                }
            default:
                var problemDetails = new ProblemDetails
                {
                    Title = "An unexpected error occurred!",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = exceptionMessage
                };
                httpContext.Response.StatusCode = problemDetails.Status.Value;
                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                return true;
        }
    }
}
