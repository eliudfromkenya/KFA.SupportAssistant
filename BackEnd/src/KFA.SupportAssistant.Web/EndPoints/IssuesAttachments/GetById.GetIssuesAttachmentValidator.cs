using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesAttachments;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetIssuesAttachmentValidator : Validator<GetIssuesAttachmentByIdRequest>
{
  public GetIssuesAttachmentValidator()
  {
    RuleFor(x => x.AttachmentID)
      .NotEmpty()
      .WithMessage("The attachment id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
