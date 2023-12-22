using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserRoles;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateUserRoleValidator : Validator<UpdateUserRoleRequest>
{
  public UpdateUserRoleValidator()
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

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.RoleId)
      .Must((args, id) => checkIds(args.RoleId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
