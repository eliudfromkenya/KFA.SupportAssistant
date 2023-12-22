using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchVerificationRightValidator : Validator<PatchVerificationRightRequest>
{
  public PatchVerificationRightValidator()
  {
    RuleFor(x => x.VerificationRightId)
     .NotEmpty()
     .WithMessage("The verification right id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
