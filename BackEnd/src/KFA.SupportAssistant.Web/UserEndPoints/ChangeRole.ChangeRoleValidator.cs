using FluentValidation;
using KFA.SupportAssistant.Infrastructure.Data.Config;

namespace KFA.SupportAssistant.Web.UserEndPoints;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class ChangeRoleValidator : Validator<ChangeRoleRequest>
{
  public ChangeRoleValidator()
  {
    RuleFor(x => x.UserId)
      .NotEmpty()
      .WithMessage("Username or user id of the role being changed is required.")
      .MinimumLength(2)
      .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    RuleFor(x => x.NewRoleId)
    .NotEmpty()
    .WithMessage("New role id is required.")
    .MinimumLength(2)
    .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);
  }
}
