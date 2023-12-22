using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesAttachments;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteIssuesAttachmentValidator : Validator<DeleteIssuesAttachmentRequest>
{
  public DeleteIssuesAttachmentValidator()
  {
    RuleFor(x => x.AttachmentID)
      .NotEmpty()
      .WithMessage("The attachment id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
