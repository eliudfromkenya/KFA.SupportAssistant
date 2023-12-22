using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateSalesBudgetDetailValidator : Validator<UpdateSalesBudgetDetailRequest>
{
  public UpdateSalesBudgetDetailValidator()
  {
    RuleFor(x => x.BatchKey)
    .MinimumLength(2)
    .MaximumLength(25);

    RuleFor(x => x.ItemCode)
         .MinimumLength(2)
         .MaximumLength(15);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.SalesBudgetDetailId)
         .NotEmpty()
         .WithMessage("Sales Budget Detail Id is required.");

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.SalesBudgetDetailId)
      .Must((args, id) => checkIds(args.SalesBudgetDetailId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
