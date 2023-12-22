using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SystemRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetSystemRightValidator : Validator<GetSystemRightByIdRequest>
{
  public GetSystemRightValidator()
  {
    RuleFor(x => x.RightId)
      .NotEmpty()
      .WithMessage("The right id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
