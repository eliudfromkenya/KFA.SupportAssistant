using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetValuations;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateComputerAssetValuationValidator : Validator<UpdateComputerAssetValuationRequest>
{
  public UpdateComputerAssetValuationValidator()
  {
    RuleFor(x => x.AssetDetailID)
    .NotEmpty()
    .WithMessage("Asset Detail ID is required.");

    RuleFor(x => x.Description)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.RevaluationID)
         .NotEmpty()
         .WithMessage("Revaluation ID is required.");

    RuleFor(x => x.Status)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Value)
         .NotEmpty()
         .WithMessage("Value is required.");

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.RevaluationID)
      .Must((args, id) => checkIds(args.RevaluationID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
