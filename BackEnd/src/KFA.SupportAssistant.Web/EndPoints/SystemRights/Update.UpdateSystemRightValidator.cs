using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SystemRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateSystemRightValidator : Validator<UpdateSystemRightRequest>
{
  public UpdateSystemRightValidator()
  {
    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.RightId)
         .NotEmpty()
         .WithMessage("Right Id is required.");

    RuleFor(x => x.RightName)
         .NotEmpty()
         .WithMessage("Right Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.RightId)
      .Must((args, id) => checkIds(args.RightId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
