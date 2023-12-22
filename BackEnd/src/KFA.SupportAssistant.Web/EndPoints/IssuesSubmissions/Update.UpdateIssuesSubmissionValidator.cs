using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesSubmissions;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateIssuesSubmissionValidator : Validator<UpdateIssuesSubmissionRequest>
{
  public UpdateIssuesSubmissionValidator()
  {
    RuleFor(x => x.IssueID)
    .NotEmpty()
    .WithMessage("Issue ID is required.")
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.SubmissionID)
         .NotEmpty()
         .WithMessage("Submission ID is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.SubmittedTo)
         .NotEmpty()
         .WithMessage("Submitted To is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.SubmittingUser)
         .NotEmpty()
         .WithMessage("Submitting User is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.TimeSubmitted)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Type)
         .MinimumLength(2)
         .MaximumLength(255);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.SubmissionID)
      .Must((args, id) => checkIds(args.SubmissionID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
