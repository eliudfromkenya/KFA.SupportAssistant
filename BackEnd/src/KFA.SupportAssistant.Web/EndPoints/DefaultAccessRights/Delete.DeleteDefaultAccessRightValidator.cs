using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DefaultAccessRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteDefaultAccessRightValidator : Validator<DeleteDefaultAccessRightRequest>
{
  public DeleteDefaultAccessRightValidator()
  {
    RuleFor(x => x.RightID)
      .NotEmpty()
      .WithMessage("The right id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
