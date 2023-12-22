using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteSalesBudgetDetailValidator : Validator<DeleteSalesBudgetDetailRequest>
{
  public DeleteSalesBudgetDetailValidator()
  {
    RuleFor(x => x.SalesBudgetDetailId)
      .NotEmpty()
      .WithMessage("The sales budget detail id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
