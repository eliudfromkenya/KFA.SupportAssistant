using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CommunicationMessages;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchCommunicationMessageValidator : Validator<PatchCommunicationMessageRequest>
{
  public PatchCommunicationMessageValidator()
  {
    RuleFor(x => x.MessageID)
     .NotEmpty()
     .WithMessage("The message id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
