using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetSalesBudgetDetailValidator : Validator<GetSalesBudgetDetailByIdRequest>
{
  public GetSalesBudgetDetailValidator()
  {
    RuleFor(x => x.SalesBudgetDetailId)
      .NotEmpty()
      .WithMessage("The sales budget detail id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
