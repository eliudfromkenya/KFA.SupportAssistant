using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchSalesBudgetDetailValidator : Validator<PatchSalesBudgetDetailRequest>
{
  public PatchSalesBudgetDetailValidator()
  {
    RuleFor(x => x.SalesBudgetDetailId)
     .NotEmpty()
     .WithMessage("The sales budget detail id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
