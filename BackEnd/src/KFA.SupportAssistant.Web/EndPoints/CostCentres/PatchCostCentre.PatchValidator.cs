using FastEndpoints;
using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;
/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchCostCentreValidator : Validator<PatchCostCentreRequest>
{
  public PatchCostCentreValidator()
  {
    RuleFor(x => x.CostCentreCode)
     .NotEmpty()
     .WithMessage("Cost centre code to be update is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
