using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerAssetTransferValidator : Validator<DeleteComputerAssetTransferRequest>
{
  public DeleteComputerAssetTransferValidator()
  {
    RuleFor(x => x.TransferID)
      .NotEmpty()
      .WithMessage("The transfer id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}

