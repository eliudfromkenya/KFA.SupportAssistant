using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchUserRightValidator : Validator<PatchUserRightRequest>
{
  public PatchUserRightValidator()
  {
    RuleFor(x => x.UserRightId)
     .NotEmpty()
     .WithMessage("The user right id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
