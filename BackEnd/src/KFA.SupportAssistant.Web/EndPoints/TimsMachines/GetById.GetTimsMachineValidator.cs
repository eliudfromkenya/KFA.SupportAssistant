using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.TimsMachines;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetTimsMachineValidator : Validator<GetTimsMachineByIdRequest>
{
  public GetTimsMachineValidator()
  {
    RuleFor(x => x.MachineID)
      .NotEmpty()
      .WithMessage("The machine id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
