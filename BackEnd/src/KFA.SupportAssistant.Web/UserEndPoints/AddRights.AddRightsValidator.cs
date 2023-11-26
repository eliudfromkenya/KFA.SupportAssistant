using FluentValidation;
using KFA.SupportAssistant.Infrastructure.Data.Config;

namespace KFA.SupportAssistant.Web.UserEndPoints;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class AddRightsValidator : Validator<AddRightsRequest>
{
  public AddRightsValidator()
  {
    RuleFor(x => x.UserId)
      .NotEmpty()
      .WithMessage("UserId or username of user to add the rights is required.")
      .MinimumLength(2)
      .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    RuleFor(o => o.Commands)
    .NotEmpty()
    .When(o => o.Rights?.Length == 0)
    .WithMessage("Please provide either commands or specific rights (group of commands) to add");

    RuleForEach(x => x.Rights).NotNull().WithMessage("Rights {CollectionIndex} is required.");
  }
}
