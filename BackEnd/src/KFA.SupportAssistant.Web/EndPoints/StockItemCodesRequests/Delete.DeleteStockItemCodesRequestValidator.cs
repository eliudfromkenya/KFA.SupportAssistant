using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StockItemCodesRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteStockItemCodesRequestValidator : Validator<DeleteStockItemCodesRequestRequest>
{
  public DeleteStockItemCodesRequestValidator()
  {
    RuleFor(x => x.ItemCodeRequestID)
      .NotEmpty()
      .WithMessage("The item code request id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
