using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StockItems;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchStockItemValidator : Validator<PatchStockItemRequest>
{
  public PatchStockItemValidator()
  {
    RuleFor(x => x.ItemCode)
     .NotEmpty()
     .WithMessage("The item code of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
