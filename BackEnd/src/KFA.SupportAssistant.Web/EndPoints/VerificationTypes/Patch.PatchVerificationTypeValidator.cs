using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationTypes;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchVerificationTypeValidator : Validator<PatchVerificationTypeRequest>
{
  public PatchVerificationTypeValidator()
  {
    RuleFor(x => x.VerificationTypeId)
     .NotEmpty()
     .WithMessage("The verification type id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
