using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationTypes;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteVerificationTypeValidator : Validator<DeleteVerificationTypeRequest>
{
  public DeleteVerificationTypeValidator()
  {
    RuleFor(x => x.VerificationTypeId)
      .NotEmpty()
      .WithMessage("The verification type id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
