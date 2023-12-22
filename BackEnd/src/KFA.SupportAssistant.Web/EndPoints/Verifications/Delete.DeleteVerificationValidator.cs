using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.Verifications;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteVerificationValidator : Validator<DeleteVerificationRequest>
{
  public DeleteVerificationValidator()
  {
    RuleFor(x => x.VerificationId)
      .NotEmpty()
      .WithMessage("The verification id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
