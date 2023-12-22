using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserRoles;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteUserRoleValidator : Validator<DeleteUserRoleRequest>
{
  public DeleteUserRoleValidator()
  {
    RuleFor(x => x.RoleId)
      .NotEmpty()
      .WithMessage("The role id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
