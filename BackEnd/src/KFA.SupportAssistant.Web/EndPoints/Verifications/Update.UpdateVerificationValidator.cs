using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.Verifications;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateVerificationValidator : Validator<UpdateVerificationRequest>
{
  public UpdateVerificationValidator()
  {
    RuleFor(x => x.DateOfVerification)
    .NotEmpty()
    .WithMessage("Date Of Verification is required.");

    RuleFor(x => x.LoginId)
         .NotEmpty()
         .WithMessage("Login Id is required.");

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.RecordId)
         .NotEmpty()
         .WithMessage("Record Id is required.");

    RuleFor(x => x.TableName)
         .NotEmpty()
         .WithMessage("Table Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.VerificationId)
         .NotEmpty()
         .WithMessage("Verification Id is required.");

    RuleFor(x => x.VerificationName)
         .NotEmpty()
         .WithMessage("Verification Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.VerificationTypeId)
         .NotEmpty()
         .WithMessage("Verification Type Id is required.");

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.VerificationId)
      .Must((args, id) => checkIds(args.VerificationId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
