using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetIssuesProgressValidator : Validator<GetIssuesProgressByIdRequest>
{
  public GetIssuesProgressValidator()
  {
    RuleFor(x => x.ProgressID)
      .NotEmpty()
      .WithMessage("The progress id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
