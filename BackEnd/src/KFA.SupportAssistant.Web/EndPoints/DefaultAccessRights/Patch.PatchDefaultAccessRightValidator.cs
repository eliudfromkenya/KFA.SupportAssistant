using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DefaultAccessRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchDefaultAccessRightValidator : Validator<PatchDefaultAccessRightRequest>
{
  public PatchDefaultAccessRightValidator()
  {
    RuleFor(x => x.RightID)
     .NotEmpty()
     .WithMessage("The right id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
