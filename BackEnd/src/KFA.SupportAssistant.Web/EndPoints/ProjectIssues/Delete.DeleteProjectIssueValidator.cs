using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ProjectIssues;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteProjectIssueValidator : Validator<DeleteProjectIssueRequest>
{
  public DeleteProjectIssueValidator()
  {
    RuleFor(x => x.ProjectIssueID)
      .NotEmpty()
      .WithMessage("The project issue id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
