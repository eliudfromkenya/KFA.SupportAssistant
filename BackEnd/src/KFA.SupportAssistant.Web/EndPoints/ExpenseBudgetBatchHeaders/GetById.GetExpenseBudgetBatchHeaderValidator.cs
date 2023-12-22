using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ExpenseBudgetBatchHeaders;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetExpenseBudgetBatchHeaderValidator : Validator<GetExpenseBudgetBatchHeaderByIdRequest>
{
  public GetExpenseBudgetBatchHeaderValidator()
  {
    RuleFor(x => x.BatchKey)
      .NotEmpty()
      .WithMessage("The batch key to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
