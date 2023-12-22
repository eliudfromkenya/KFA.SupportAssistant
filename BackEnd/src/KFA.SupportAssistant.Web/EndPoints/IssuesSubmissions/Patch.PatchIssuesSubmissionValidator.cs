using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesSubmissions;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchIssuesSubmissionValidator : Validator<PatchIssuesSubmissionRequest>
{
  public PatchIssuesSubmissionValidator()
  {
    RuleFor(x => x.SubmissionID)
     .NotEmpty()
     .WithMessage("The submission id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
