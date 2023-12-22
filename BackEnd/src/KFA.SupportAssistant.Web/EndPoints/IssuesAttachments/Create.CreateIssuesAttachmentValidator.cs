using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesAttachments;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateIssuesAttachmentValidator : Validator<CreateIssuesAttachmentRequest>
{
  public CreateIssuesAttachmentValidator()
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
  }
}
