using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserRoles;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateUserRoleValidator : Validator<CreateUserRoleRequest>
{
  public CreateUserRoleValidator()
  {
    RuleFor(x => x.ExpirationDate)
    .NotEmpty()
    .WithMessage("Expiration Date is required.");

    RuleFor(x => x.MaturityDate)
         .NotEmpty()
         .WithMessage("Maturity Date is required.");

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.RoleId)
         .NotEmpty()
         .WithMessage("Role Id is required.");

    RuleFor(x => x.RoleName)
         .NotEmpty()
         .WithMessage("Role Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
