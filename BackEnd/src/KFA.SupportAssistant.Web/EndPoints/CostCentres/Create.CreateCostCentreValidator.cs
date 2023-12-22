using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateCostCentreValidator : Validator<CreateCostCentreRequest>
{
  public CreateCostCentreValidator()
  {
    RuleFor(x => x.CostCentreCode)
    .NotEmpty()
    .WithMessage("Cost Centre Code is required.");

    RuleFor(x => x.Description)
         .NotEmpty()
         .WithMessage("Description is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.Region)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.SupplierCodePrefix)
         .MinimumLength(2)
         .MaximumLength(10);
  }
}
