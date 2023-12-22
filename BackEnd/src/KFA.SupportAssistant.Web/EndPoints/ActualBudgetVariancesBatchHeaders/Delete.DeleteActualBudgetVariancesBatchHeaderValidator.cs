using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariancesBatchHeaders;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteActualBudgetVariancesBatchHeaderValidator : Validator<DeleteActualBudgetVariancesBatchHeaderRequest>
{
  public DeleteActualBudgetVariancesBatchHeaderValidator()
  {
    RuleFor(x => x.BatchKey)
      .NotEmpty()
      .WithMessage("The batch key to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
