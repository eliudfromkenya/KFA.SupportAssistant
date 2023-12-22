using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CommandDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateCommandDetailValidator : Validator<CreateCommandDetailRequest>
{
  public CreateCommandDetailValidator()
  {
    RuleFor(x => x.Action)
    .NotEmpty()
    .WithMessage("Action is required.")
    .MinimumLength(2)
    .MaximumLength(25);

    RuleFor(x => x.ActiveState)
         .NotEmpty()
         .WithMessage("Active State is required.")
         .MinimumLength(2)
         .MaximumLength(10);

    RuleFor(x => x.Category)
         .NotEmpty()
         .WithMessage("Category is required.")
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.CommandId)
         .NotEmpty()
         .WithMessage("Command Id is required.");

    RuleFor(x => x.CommandName)
         .NotEmpty()
         .WithMessage("Command Name is required.")
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.CommandText)
         .NotEmpty()
         .WithMessage("Command Text is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ImageId)
         .NotEmpty()
         .WithMessage("Image Id is required.");

    RuleFor(x => x.ImagePath)
         .NotEmpty()
         .WithMessage("Image Path is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.ShortcutKey)
         .NotEmpty()
         .WithMessage("Shortcut Key is required.")
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
