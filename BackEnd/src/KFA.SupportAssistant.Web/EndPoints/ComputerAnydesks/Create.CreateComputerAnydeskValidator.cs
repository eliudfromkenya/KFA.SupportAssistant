using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAnydesks;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateComputerAnydeskValidator : Validator<CreateComputerAnydeskRequest>
{
  public CreateComputerAnydeskValidator()
  {
    RuleFor(x => x.AnyDeskId)
    .NotEmpty()
    .WithMessage("AnyDesk Id is required.");

    RuleFor(x => x.AnyDeskNumber)
         .NotEmpty()
         .WithMessage("AnyDesk Number is required.")
         .MinimumLength(2)
         .MaximumLength(100);

    RuleFor(x => x.CostCentreCode)
         .NotEmpty()
         .WithMessage("Cost Centre Code is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.DeviceName)
         .NotEmpty()
         .WithMessage("Device Name is required.")
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.NameOfUser)
         .NotEmpty()
         .WithMessage("Name Of User is required.")
         .MinimumLength(2)
         .MaximumLength(100);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.Password)
         .NotEmpty()
         .WithMessage("Password is required.")
         .MinimumLength(2)
         .MaximumLength(40);

    RuleFor(x => x.Type)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
