using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariancesBatchHeaders;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetActualBudgetVariancesBatchHeaderValidator : Validator<GetActualBudgetVariancesBatchHeaderByIdRequest>
{
  public GetActualBudgetVariancesBatchHeaderValidator()
  {
    RuleFor(x => x.BatchKey)
      .NotEmpty()
      .WithMessage("The batch key to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
