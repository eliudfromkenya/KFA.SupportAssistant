using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserLogins;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateUserLoginValidator : Validator<UpdateUserLoginRequest>
{
  public UpdateUserLoginValidator()
  {
    RuleFor(x => x.DeviceId)
    .NotEmpty()
    .WithMessage("Device Id is required.");

    RuleFor(x => x.FromDate)
         .NotEmpty()
         .WithMessage("From Date is required.");

    RuleFor(x => x.LoginId)
         .NotEmpty()
         .WithMessage("Login Id is required.");

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.UptoDate)
         .NotEmpty()
         .WithMessage("Upto Date is required.");

    RuleFor(x => x.UserId)
         .NotEmpty()
         .WithMessage("User Id is required.");

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.LoginId)
      .Must((args, id) => checkIds(args.LoginId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
