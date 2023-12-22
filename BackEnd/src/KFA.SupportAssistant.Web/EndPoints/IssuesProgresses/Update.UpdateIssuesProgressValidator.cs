using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateIssuesProgressValidator : Validator<UpdateIssuesProgressRequest>
{
  public UpdateIssuesProgressValidator()
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

    RuleFor(x => x.Status)
         .MinimumLength(2)
         .MaximumLength(255);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.ProgressID)
      .Must((args, id) => checkIds(args.ProgressID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
