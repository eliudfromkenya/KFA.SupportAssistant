using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesAttachments;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateIssuesAttachmentValidator : Validator<UpdateIssuesAttachmentRequest>
{
  public UpdateIssuesAttachmentValidator()
  {
    RuleFor(x => x.AttachmentID)
    .NotEmpty()
    .WithMessage("Attachment ID is required.");

    RuleFor(x => x.AttachmentType)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Description)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.File)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.AttachmentID)
      .Must((args, id) => checkIds(args.AttachmentID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
