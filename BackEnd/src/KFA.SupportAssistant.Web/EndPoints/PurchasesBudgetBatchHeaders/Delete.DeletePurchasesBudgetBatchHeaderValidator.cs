using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetBatchHeaders;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeletePurchasesBudgetBatchHeaderValidator : Validator<DeletePurchasesBudgetBatchHeaderRequest>
{
  public DeletePurchasesBudgetBatchHeaderValidator()
  {
    RuleFor(x => x.BatchKey)
      .NotEmpty()
      .WithMessage("The batch key to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
