using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetBatchHeaders;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetPurchasesBudgetBatchHeaderValidator : Validator<GetPurchasesBudgetBatchHeaderByIdRequest>
{
  public GetPurchasesBudgetBatchHeaderValidator()
  {
    RuleFor(x => x.BatchKey)
      .NotEmpty()
      .WithMessage("The batch key to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
