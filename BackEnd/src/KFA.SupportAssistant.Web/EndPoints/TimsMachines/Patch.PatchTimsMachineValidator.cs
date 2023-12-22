using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.TimsMachines;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchTimsMachineValidator : Validator<PatchTimsMachineRequest>
{
  public PatchTimsMachineValidator()
  {
    RuleFor(x => x.MachineID)
     .NotEmpty()
     .WithMessage("The machine id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
