using FluentValidation;
using KFA.SupportAssistant.Infrastructure.Data.Config;

namespace KFA.SupportAssistant.Web.UserEndPoints;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class ClearRightsValidator : Validator<ClearRightsRequest>
{
  public ClearRightsValidator()
  {
    RuleFor(x => x.UserId)
      .NotEmpty()
      .WithMessage("Username or user id of which rights are to be cleared is required.")
      .MinimumLength(2)
      .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    RuleFor(x => x.UserRightIds)
    .NotEmpty()
    .WithMessage("Rights to clear is required.");
  }
}
