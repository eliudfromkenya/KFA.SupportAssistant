using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CommunicationMessages;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteCommunicationMessageValidator : Validator<DeleteCommunicationMessageRequest>
{
  public DeleteCommunicationMessageValidator()
  {
    RuleFor(x => x.MessageID)
      .NotEmpty()
      .WithMessage("The message id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
