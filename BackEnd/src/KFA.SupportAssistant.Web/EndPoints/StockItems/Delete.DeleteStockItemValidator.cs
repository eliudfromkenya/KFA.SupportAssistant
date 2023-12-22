using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StockItems;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteStockItemValidator : Validator<DeleteStockItemRequest>
{
  public DeleteStockItemValidator()
  {
    RuleFor(x => x.ItemCode)
      .NotEmpty()
      .WithMessage("The item code to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
