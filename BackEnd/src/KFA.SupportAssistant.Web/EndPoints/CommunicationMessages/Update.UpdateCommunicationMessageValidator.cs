using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CommunicationMessages;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateCommunicationMessageValidator : Validator<UpdateCommunicationMessageRequest>
{
  public UpdateCommunicationMessageValidator()
  {
    RuleFor(x => x.Details)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.From)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Message)
         .NotEmpty()
         .WithMessage("Message is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.MessageID)
         .NotEmpty()
         .WithMessage("Message ID is required.");

    RuleFor(x => x.MessageType)
         .NotEmpty()
         .WithMessage("Message Type is required.");

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.Title)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.To)
         .MinimumLength(2)
         .MaximumLength(255);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.MessageID)
      .Must((args, id) => checkIds(args.MessageID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
