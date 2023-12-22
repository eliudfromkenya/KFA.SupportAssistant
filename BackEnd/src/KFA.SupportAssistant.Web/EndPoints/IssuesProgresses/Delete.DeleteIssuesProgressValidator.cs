using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteIssuesProgressValidator : Validator<DeleteIssuesProgressRequest>
{
  public DeleteIssuesProgressValidator()
  {
    RuleFor(x => x.ProgressID)
      .NotEmpty()
      .WithMessage("The progress id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
