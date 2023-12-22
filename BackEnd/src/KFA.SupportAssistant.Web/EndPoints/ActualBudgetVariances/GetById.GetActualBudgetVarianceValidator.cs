using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariances;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetActualBudgetVarianceValidator : Validator<GetActualBudgetVarianceByIdRequest>
{
  public GetActualBudgetVarianceValidator()
  {
    RuleFor(x => x.ActualBudgetID)
      .NotEmpty()
      .WithMessage("The actual budget id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
