using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StockItems;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetStockItemValidator : Validator<GetStockItemByIdRequest>
{
  public GetStockItemValidator()
  {
    RuleFor(x => x.ItemCode)
      .NotEmpty()
      .WithMessage("The item code to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
