using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesSubmissions;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteIssuesSubmissionValidator : Validator<DeleteIssuesSubmissionRequest>
{
  public DeleteIssuesSubmissionValidator()
  {
    RuleFor(x => x.SubmissionID)
      .NotEmpty()
      .WithMessage("The submission id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
