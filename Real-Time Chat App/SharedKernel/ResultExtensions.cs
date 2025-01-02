using Microsoft.AspNetCore.Mvc;
using RealTimeChatApp.Domain.Shared;
using System.Xml.Linq;

namespace Real_Time_Chat_App.SharedKernel;

public static class ResultExtensions
{
    public static IActionResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }
        var ValidationResult = result as ValidationResult;
        //Error[] errors = ValidationResult?.Errors?.ToArray() ?? [ValidationResult.Error];
        Error[] errors = ValidationResult?.Errors is not null ? ValidationResult.Errors : [result.Error];
        var problemDetails = new ProblemDetails {
            Status = GetStatusCode(result.Error.Type),
            Title = GetTitle(result.Error.Type),
            Extensions = new Dictionary<string, object?>
            {
                {"errors", new[]{errors} }
            }
            };

        static int GetStatusCode(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.UnAuthorized => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError,
            };

        static string GetTitle(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Validation => "Bad Request",
                ErrorType.NotFound => "Not Found",
                ErrorType.Conflict => "Conflict",
                ErrorType.UnAuthorized => "Unauthorized",
                _ => "Server Failure",
            };
        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }
}
