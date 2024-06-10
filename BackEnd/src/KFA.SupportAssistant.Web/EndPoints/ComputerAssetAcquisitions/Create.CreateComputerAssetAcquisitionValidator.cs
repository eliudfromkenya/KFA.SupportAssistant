using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateComputerAssetAcquisitionValidator : Validator<CreateComputerAssetAcquisitionRequest>
{
  public CreateComputerAssetAcquisitionValidator()
  {
    RuleFor(x => x.AcquisitionID)
    .NotEmpty()
    .WithMessage("Acquisition ID is required.");

    RuleFor(x => x.AquisitionType)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.DocumentNo)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.QuotationNumber)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ReceivedBy)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.VendorCode)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
