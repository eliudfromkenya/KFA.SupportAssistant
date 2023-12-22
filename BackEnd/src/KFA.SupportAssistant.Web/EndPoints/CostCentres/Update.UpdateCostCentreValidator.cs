using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateCostCentreValidator : Validator<UpdateCostCentreRequest>
{
  public UpdateCostCentreValidator()
  {
    RuleFor(x => x.CostCentreCode)
    .NotEmpty()
    .WithMessage("Cost Centre Code is required.");

    RuleFor(x => x.Description)
         .NotEmpty()
         .WithMessage("Description is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.Region)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.SupplierCodePrefix)
         .MinimumLength(2)
         .MaximumLength(10);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.CostCentreCode)
      .Must((args, id) => checkIds(args.CostCentreCode, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
