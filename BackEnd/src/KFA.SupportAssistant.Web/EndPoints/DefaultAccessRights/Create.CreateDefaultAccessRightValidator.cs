using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DefaultAccessRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateDefaultAccessRightValidator : Validator<CreateDefaultAccessRightRequest>
{
  public CreateDefaultAccessRightValidator()
  {
    RuleFor(x => x.Name)
    .NotEmpty()
    .WithMessage("Name is required.")
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.RightID)
         .NotEmpty()
         .WithMessage("Right ID is required.");

    RuleFor(x => x.Rights)
         .NotEmpty()
         .WithMessage("Rights is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Type)
         .NotEmpty()
         .WithMessage("Type is required.")
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
