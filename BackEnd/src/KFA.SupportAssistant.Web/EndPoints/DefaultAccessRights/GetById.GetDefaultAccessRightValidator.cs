using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DefaultAccessRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetDefaultAccessRightValidator : Validator<GetDefaultAccessRightByIdRequest>
{
  public GetDefaultAccessRightValidator()
  {
    RuleFor(x => x.RightID)
      .NotEmpty()
      .WithMessage("The right id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
