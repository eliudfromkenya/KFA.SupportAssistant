using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetComputerAssetTransferValidator : Validator<GetComputerAssetTransferByIdRequest>
{
  public GetComputerAssetTransferValidator()
  {
    RuleFor(x => x.TransferID)
      .NotEmpty()
      .WithMessage("The transfer id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
