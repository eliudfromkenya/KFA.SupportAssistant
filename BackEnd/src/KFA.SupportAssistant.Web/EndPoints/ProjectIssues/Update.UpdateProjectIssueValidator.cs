using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ProjectIssues;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateProjectIssueValidator : Validator<UpdateProjectIssueRequest>
{
  public UpdateProjectIssueValidator()
  {
    RuleFor(x => x.Category)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.Description)
         .NotEmpty()
         .WithMessage("Description is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Effect)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.ProjectIssueID)
         .NotEmpty()
         .WithMessage("Project Issue ID is required.");

    RuleFor(x => x.RegisteredBy)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.SubCategory)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Title)
         .NotEmpty()
         .WithMessage("Title is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.ProjectIssueID)
      .Must((args, id) => checkIds(args.ProjectIssueID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
