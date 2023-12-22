using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SystemUsers;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetSystemUserValidator : Validator<GetSystemUserByIdRequest>
{
  public GetSystemUserValidator()
  {
    RuleFor(x => x.UserId)
      .NotEmpty()
      .WithMessage("The user id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
