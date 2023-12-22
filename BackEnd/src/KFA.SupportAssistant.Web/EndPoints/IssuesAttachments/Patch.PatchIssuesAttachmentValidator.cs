using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesAttachments;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchIssuesAttachmentValidator : Validator<PatchIssuesAttachmentRequest>
{
  public PatchIssuesAttachmentValidator()
  {
    RuleFor(x => x.AttachmentID)
     .NotEmpty()
     .WithMessage("The attachment id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
