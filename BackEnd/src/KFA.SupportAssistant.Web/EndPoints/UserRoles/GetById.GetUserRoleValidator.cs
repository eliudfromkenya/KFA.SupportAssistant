using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserRoles;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetUserRoleValidator : Validator<GetUserRoleByIdRequest>
{
  public GetUserRoleValidator()
  {
    RuleFor(x => x.RoleId)
      .NotEmpty()
      .WithMessage("The role id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
