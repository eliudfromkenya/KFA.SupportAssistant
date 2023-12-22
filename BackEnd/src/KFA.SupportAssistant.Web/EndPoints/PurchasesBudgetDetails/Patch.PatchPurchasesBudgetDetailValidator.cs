using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchPurchasesBudgetDetailValidator : Validator<PatchPurchasesBudgetDetailRequest>
{
  public PatchPurchasesBudgetDetailValidator()
  {
    RuleFor(x => x.PurchasesBudgetDetailId)
     .NotEmpty()
     .WithMessage("The purchases budget detail id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
