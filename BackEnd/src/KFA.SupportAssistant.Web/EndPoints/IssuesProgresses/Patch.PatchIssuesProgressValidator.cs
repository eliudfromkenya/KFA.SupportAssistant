using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchIssuesProgressValidator : Validator<PatchIssuesProgressRequest>
{
  public PatchIssuesProgressValidator()
  {
    RuleFor(x => x.ProgressID)
     .NotEmpty()
     .WithMessage("The progress id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
