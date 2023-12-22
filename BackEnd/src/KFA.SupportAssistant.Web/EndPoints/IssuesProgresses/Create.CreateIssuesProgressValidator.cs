using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateIssuesProgressValidator : Validator<CreateIssuesProgressRequest>
{
  public CreateIssuesProgressValidator()
  {
    RuleFor(x => x.Description)
    .NotEmpty()
    .WithMessage("Description is required.")
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.IssueID)
         .NotEmpty()
         .WithMessage("Issue ID is required.");

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.ProgressID)
         .NotEmpty()
         .WithMessage("Progress ID is required.");

    RuleFor(x => x.ReportedBy)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
