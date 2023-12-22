using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetCostCentreValidator : Validator<GetCostCentreByIdRequest>
{
  public GetCostCentreValidator()
  {
    RuleFor(x => x.CostCentreCode)
      .NotEmpty()
      .WithMessage("The cost centre code to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
