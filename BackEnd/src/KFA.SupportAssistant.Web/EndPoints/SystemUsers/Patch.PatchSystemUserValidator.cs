using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SystemUsers;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchSystemUserValidator : Validator<PatchSystemUserRequest>
{
  public PatchSystemUserValidator()
  {
    RuleFor(x => x.UserId)
     .NotEmpty()
     .WithMessage("The user id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
