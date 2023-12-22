using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StockItems;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateStockItemValidator : Validator<CreateStockItemRequest>
{
  public CreateStockItemValidator()
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
  }
}
