using Ardalis.Result;
using FastEndpoints;

namespace KFA.SupportAssistant.Infrastructure.Services;

public static class ErrorsConverter
{
  public static async Task CheckErrors(HttpContext context, ResultStatus error, IEnumerable<string> errors, CancellationToken cancellationToken)
  {
    var status = error switch
    {
      ResultStatus.Ok => StatusCodes.Status200OK,
      ResultStatus.Error => StatusCodes.Status417ExpectationFailed,
      ResultStatus.Forbidden => StatusCodes.Status403Forbidden,
      ResultStatus.Unauthorized => StatusCodes.Status401Unauthorized,
      ResultStatus.Invalid => StatusCodes.Status400BadRequest,
      ResultStatus.NotFound => StatusCodes.Status404NotFound,
      ResultStatus.Conflict => StatusCodes.Status409Conflict,
      ResultStatus.CriticalError => StatusCodes.Status500InternalServerError,
      ResultStatus.Unavailable => StatusCodes.Status451UnavailableForLegalReasons,
      _ => StatusCodes.Status417ExpectationFailed,
    };

    var resultErrors = errors.ToList().Select(c => new FluentValidation.Results.ValidationFailure(nameof(string.Empty), c)).ToList();
    await context.Response.SendErrorsAsync(resultErrors, cancellation: cancellationToken, statusCode: status);
    return;
  }
}
