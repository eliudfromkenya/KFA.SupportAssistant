using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetBatchHeaders;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetSalesBudgetBatchHeaderValidator : Validator<GetSalesBudgetBatchHeaderByIdRequest>
{
  public GetSalesBudgetBatchHeaderValidator()
  {
    RuleFor(x => x.BatchKey)
      .NotEmpty()
      .WithMessage("The batch key to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
