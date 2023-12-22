using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.Verifications;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchVerificationValidator : Validator<PatchVerificationRequest>
{
  public PatchVerificationValidator()
  {
    RuleFor(x => x.VerificationId)
     .NotEmpty()
     .WithMessage("The verification id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
