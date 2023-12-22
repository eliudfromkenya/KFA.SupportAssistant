using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StockItemCodesRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetStockItemCodesRequestValidator : Validator<GetStockItemCodesRequestByIdRequest>
{
  public GetStockItemCodesRequestValidator()
  {
    RuleFor(x => x.ItemCodeRequestID)
      .NotEmpty()
      .WithMessage("The item code request id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
