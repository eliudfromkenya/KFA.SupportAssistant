using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateVerificationRightValidator : Validator<UpdateVerificationRightRequest>
{
  public UpdateVerificationRightValidator()
  {
    RuleFor(x => x.UserRoleId)
    .NotEmpty()
    .WithMessage("User Role Id is required.");

    RuleFor(x => x.VerificationRightId)
         .NotEmpty()
         .WithMessage("Verification Right Id is required.");

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.VerificationRightId)
      .Must((args, id) => checkIds(args.VerificationRightId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
