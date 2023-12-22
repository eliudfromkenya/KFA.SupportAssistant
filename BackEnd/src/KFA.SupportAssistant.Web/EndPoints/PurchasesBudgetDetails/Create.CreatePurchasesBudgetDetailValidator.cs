using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreatePurchasesBudgetDetailValidator : Validator<CreatePurchasesBudgetDetailRequest>
{
  public CreatePurchasesBudgetDetailValidator()
  {
    RuleFor(x => x.ItemCode)
    .MinimumLength(2)
    .MaximumLength(15);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.PurchasesBudgetDetailId)
         .NotEmpty()
         .WithMessage("Purchases Budget Detail Id is required.");
  }
}
