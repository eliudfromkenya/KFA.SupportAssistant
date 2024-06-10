using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchComputerAssetTransferValidator : Validator<PatchComputerAssetTransferRequest>
{
  public PatchComputerAssetTransferValidator()
  {
    RuleFor(x => x.TransferID)
     .NotEmpty()
     .WithMessage("The transfer id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
