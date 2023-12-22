using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationTypes;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateVerificationTypeValidator : Validator<UpdateVerificationTypeRequest>
{
  public UpdateVerificationTypeValidator()
  {
    RuleFor(x => x.Category)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.VerificationTypeId)
         .NotEmpty()
         .WithMessage("Verification Type Id is required.");

    RuleFor(x => x.VerificationTypeName)
         .NotEmpty()
         .WithMessage("Verification Type Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.VerificationTypeId)
      .Must((args, id) => checkIds(args.VerificationTypeId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
