using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserRoles;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchUserRoleValidator : Validator<PatchUserRoleRequest>
{
  public PatchUserRoleValidator()
  {
    RuleFor(x => x.RoleId)
     .NotEmpty()
     .WithMessage("The role id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
