using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetUserRightValidator : Validator<GetUserRightByIdRequest>
{
  public GetUserRightValidator()
  {
    RuleFor(x => x.UserRightId)
      .NotEmpty()
      .WithMessage("The user right id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
