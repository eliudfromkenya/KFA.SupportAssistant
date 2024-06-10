using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchComputerAssetsMaintainceReceivingValidator : Validator<PatchComputerAssetsMaintainceReceivingRequest>
{
  public PatchComputerAssetsMaintainceReceivingValidator()
  {
    RuleFor(x => x.ReceiveID)
     .NotEmpty()
     .WithMessage("The receive id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
