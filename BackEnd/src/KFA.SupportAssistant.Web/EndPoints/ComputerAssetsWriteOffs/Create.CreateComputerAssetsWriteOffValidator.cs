using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateComputerAssetsWriteOffValidator : Validator<CreateComputerAssetsWriteOffRequest>
{
  public CreateComputerAssetsWriteOffValidator()
  {
     RuleFor(x => x.Description)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.Narration)
     .MinimumLength(2)
     .MaximumLength(500);

RuleFor(x => x.ReasonForWriteOff)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.WriteOffID)
     .NotEmpty()
     .WithMessage("Write Off ID is required.");             
  }
}