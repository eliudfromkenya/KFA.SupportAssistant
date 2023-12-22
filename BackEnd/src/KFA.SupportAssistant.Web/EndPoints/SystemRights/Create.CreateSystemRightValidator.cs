using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SystemRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateSystemRightValidator : Validator<CreateSystemRightRequest>
{
  public CreateSystemRightValidator()
  {
     RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.RightId)
         .NotEmpty()
         .WithMessage("Right Id is required.");

    RuleFor(x => x.RightName)
         .NotEmpty()
         .WithMessage("Right Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
