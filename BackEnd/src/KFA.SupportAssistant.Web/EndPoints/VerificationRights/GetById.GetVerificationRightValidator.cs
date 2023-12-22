using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetVerificationRightValidator : Validator<GetVerificationRightByIdRequest>
{
  public GetVerificationRightValidator()
  {
    RuleFor(x => x.VerificationRightId)
      .NotEmpty()
      .WithMessage("The verification right id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
