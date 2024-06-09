using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchComputerAssetsMaintainceDispatchValidator : Validator<PatchComputerAssetsMaintainceDispatchRequest>
{
  public PatchComputerAssetsMaintainceDispatchValidator()
  {
    RuleFor(x => x.DispatchID)
     .NotEmpty()
     .WithMessage("The dispatch id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}

