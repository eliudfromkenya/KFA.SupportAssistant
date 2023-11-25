using FastEndpoints;
using FluentValidation;

namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateCostCentreValidator : Validator<CreateCostCentreRequest>
{
  public CreateCostCentreValidator()
  {
    RuleFor(x => x.Description)
     .NotEmpty()
     .WithMessage("Name is required.")
     .MinimumLength(2)
     .MaximumLength(30);
  }
}
