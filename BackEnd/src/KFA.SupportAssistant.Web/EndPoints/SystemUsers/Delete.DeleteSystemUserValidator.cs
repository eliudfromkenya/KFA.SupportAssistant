using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SystemUsers;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteSystemUserValidator : Validator<DeleteSystemUserRequest>
{
  public DeleteSystemUserValidator()
  {
    RuleFor(x => x.UserId)
      .NotEmpty()
      .WithMessage("The user id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
