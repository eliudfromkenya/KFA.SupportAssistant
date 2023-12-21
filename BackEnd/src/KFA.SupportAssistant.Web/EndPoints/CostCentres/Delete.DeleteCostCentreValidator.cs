using FluentValidation;

namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteCostCentreValidator : Validator<DeleteCostCentreRequest>
{
  public DeleteCostCentreValidator()
  {
    RuleFor(x => x.CostCentreCode)
      .NotEmpty()
      .WithMessage("The cost centre code to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
