using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetPurchasesBudgetDetailValidator : Validator<GetPurchasesBudgetDetailByIdRequest>
{
  public GetPurchasesBudgetDetailValidator()
  {
    RuleFor(x => x.PurchasesBudgetDetailId)
      .NotEmpty()
      .WithMessage("The purchases budget detail id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
