using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerAssetsMaintainceDispatchValidator : Validator<DeleteComputerAssetsMaintainceDispatchRequest>
{
  public DeleteComputerAssetsMaintainceDispatchValidator()
  {
    RuleFor(x => x.DispatchID)
      .NotEmpty()
      .WithMessage("The dispatch id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}

