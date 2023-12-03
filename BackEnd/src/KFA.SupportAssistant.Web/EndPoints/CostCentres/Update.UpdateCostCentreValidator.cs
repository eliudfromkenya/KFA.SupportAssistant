using FastEndpoints;
using FluentValidation;
using KFA.SupportAssistant.Infrastructure.Data.Config;

namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateCostCentreValidator : Validator<UpdateCostCentreRequest>
{
  public UpdateCostCentreValidator()
  {
    //RuleFor(x => x.CostCentreCode)
    //  .NotEmpty()
    //  .WithMessage("Cost centre code of the cost centre to update is required please")
    //  .MinimumLength(2)
    //  .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    RuleFor(x => x.Description)
     .NotEmpty()
     .WithMessage("Name of cost centre to update is required please.")
     .MinimumLength(2)
     .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.CostCentreCode)
      .Must((args, costCentreId) => checkIds(args.CostCentreCode, costCentreId))
      .WithMessage("Route and body Ids must match; cannot update Id of an existing resource.");
  }
}
