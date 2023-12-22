using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateSalesBudgetDetailValidator : Validator<CreateSalesBudgetDetailRequest>
{
  public CreateSalesBudgetDetailValidator()
  {
    RuleFor(x => x.BatchKey)
    .MinimumLength(2)
    .MaximumLength(25);

    RuleFor(x => x.ItemCode)
         .MinimumLength(2)
         .MaximumLength(15);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.SalesBudgetDetailId)
         .NotEmpty()
         .WithMessage("Sales Budget Detail Id is required.");
  }
}
