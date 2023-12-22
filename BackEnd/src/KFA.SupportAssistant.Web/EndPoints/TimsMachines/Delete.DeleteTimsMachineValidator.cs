using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.TimsMachines;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteTimsMachineValidator : Validator<DeleteTimsMachineRequest>
{
  public DeleteTimsMachineValidator()
  {
    RuleFor(x => x.MachineID)
      .NotEmpty()
      .WithMessage("The machine id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
