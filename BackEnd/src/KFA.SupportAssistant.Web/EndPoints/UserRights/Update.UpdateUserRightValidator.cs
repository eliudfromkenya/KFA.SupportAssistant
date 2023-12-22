using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateUserRightValidator : Validator<UpdateUserRightRequest>
{
  public UpdateUserRightValidator()
  {
    RuleFor(x => x.Description)
    .NotEmpty()
    .WithMessage("Description is required.")
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.ObjectName)
         .NotEmpty()
         .WithMessage("Object Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.RightId)
         .NotEmpty()
         .WithMessage("Right Id is required.");

    RuleFor(x => x.RoleId)
         .NotEmpty()
         .WithMessage("Role Id is required.");

    RuleFor(x => x.UserId)
         .NotEmpty()
         .WithMessage("User Id is required.");

    RuleFor(x => x.UserRightId)
         .NotEmpty()
         .WithMessage("User Right Id is required.");

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.UserRightId)
      .Must((args, id) => checkIds(args.UserRightId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
