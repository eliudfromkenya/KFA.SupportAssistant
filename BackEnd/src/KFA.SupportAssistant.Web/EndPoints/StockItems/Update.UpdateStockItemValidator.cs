using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StockItems;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateStockItemValidator : Validator<UpdateStockItemRequest>
{
  public UpdateStockItemValidator()
  {
    RuleFor(x => x.Barcode)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.ItemCode)
         .NotEmpty()
         .WithMessage("Item Code is required.")
         .MinimumLength(2)
         .MaximumLength(10);

    RuleFor(x => x.ItemName)
         .NotEmpty()
         .WithMessage("Item Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.ItemCode)
      .Must((args, id) => checkIds(args.ItemCode, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
