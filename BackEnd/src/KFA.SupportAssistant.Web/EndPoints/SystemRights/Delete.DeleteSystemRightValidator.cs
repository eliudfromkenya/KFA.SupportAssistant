using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SystemRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteSystemRightValidator : Validator<DeleteSystemRightRequest>
{
  public DeleteSystemRightValidator()
  {
    RuleFor(x => x.RightId)
      .NotEmpty()
      .WithMessage("The right id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
