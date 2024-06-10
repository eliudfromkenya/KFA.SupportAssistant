using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateComputerAssetTransferValidator : Validator<UpdateComputerAssetTransferRequest>
{
  public UpdateComputerAssetTransferValidator()
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

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.TransferID)
      .Must((args, id) => checkIds(args.TransferID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
