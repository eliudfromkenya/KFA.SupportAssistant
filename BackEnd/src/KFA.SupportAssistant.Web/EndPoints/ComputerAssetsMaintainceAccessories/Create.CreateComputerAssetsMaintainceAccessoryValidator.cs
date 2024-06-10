using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceAccessories;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateComputerAssetsMaintainceAccessoryValidator : Validator<CreateComputerAssetsMaintainceAccessoryRequest>
{
  public CreateComputerAssetsMaintainceAccessoryValidator()
  {
    RuleFor(x => x.AccessoryGroupID)
    .NotEmpty()
    .WithMessage("Accessory Group ID is required.");

    RuleFor(x => x.AccessoryID)
         .NotEmpty()
         .WithMessage("Accessory ID is required.");

    RuleFor(x => x.AssetDetailID)
         .NotEmpty()
         .WithMessage("Asset Detail ID is required.");

    RuleFor(x => x.Description)
         .NotEmpty()
         .WithMessage("Description is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.InvoiceNumber)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.QuotationNumber)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.VendorCode)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
