using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteUserRightValidator : Validator<DeleteUserRightRequest>
{
  public DeleteUserRightValidator()
  {
    RuleFor(x => x.UserRightId)
      .NotEmpty()
      .WithMessage("The user right id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
