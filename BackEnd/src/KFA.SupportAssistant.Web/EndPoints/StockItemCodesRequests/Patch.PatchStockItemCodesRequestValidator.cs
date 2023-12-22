using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StockItemCodesRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchStockItemCodesRequestValidator : Validator<PatchStockItemCodesRequestRequest>
{
  public PatchStockItemCodesRequestValidator()
  {
    RuleFor(x => x.ItemCodeRequestID)
     .NotEmpty()
     .WithMessage("The item code request id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
