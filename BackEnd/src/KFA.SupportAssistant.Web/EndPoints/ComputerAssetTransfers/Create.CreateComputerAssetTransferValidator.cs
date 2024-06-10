using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateComputerAssetTransferValidator : Validator<CreateComputerAssetTransferRequest>
{
  public CreateComputerAssetTransferValidator()
  {
    RuleFor(x => x.CostCentreCode)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.PayrollNumber)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ResponsibleUser)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Status)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.TransferID)
         .NotEmpty()
         .WithMessage("Transfer ID is required.");

    RuleFor(x => x.TransferReasons)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
