using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateComputerAssetDetailValidator : Validator<UpdateComputerAssetDetailRequest>
{
  public UpdateComputerAssetDetailValidator()
  {
    RuleFor(x => x.AssetID)
    .NotEmpty()
    .WithMessage("Asset ID is required.")
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.AssetName)
         .NotEmpty()
         .WithMessage("Asset Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Description)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.GroupID)
         .NotEmpty()
         .WithMessage("Group ID is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.SerialNumber)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.State)
         .MinimumLength(2)
         .MaximumLength(255);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.AssetID)
      .Must((args, id) => checkIds(args.AssetID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
