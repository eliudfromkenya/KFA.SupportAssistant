using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateComputerAssetAcquisitionValidator : Validator<UpdateComputerAssetAcquisitionRequest>
{
  public UpdateComputerAssetAcquisitionValidator()
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

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.AcquisitionID)
      .Must((args, id) => checkIds(args.AcquisitionID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
