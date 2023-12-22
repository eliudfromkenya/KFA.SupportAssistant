using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetBatchHeaders;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteSalesBudgetBatchHeaderValidator : Validator<DeleteSalesBudgetBatchHeaderRequest>
{
  public DeleteSalesBudgetBatchHeaderValidator()
  {
    RuleFor(x => x.BatchKey)
      .NotEmpty()
      .WithMessage("The batch key to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
