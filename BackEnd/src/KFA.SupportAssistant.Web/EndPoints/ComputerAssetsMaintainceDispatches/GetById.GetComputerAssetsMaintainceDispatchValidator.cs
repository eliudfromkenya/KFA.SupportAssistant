using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetComputerAssetsMaintainceDispatchValidator : Validator<GetComputerAssetsMaintainceDispatchByIdRequest>
{
  public GetComputerAssetsMaintainceDispatchValidator()
  {
    RuleFor(x => x.DispatchID)
      .NotEmpty()
      .WithMessage("The dispatch id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
