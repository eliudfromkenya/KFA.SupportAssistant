using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateQRRequestItemValidator : Validator<CreateQRRequestItemRequest>
{
  public CreateQRRequestItemValidator()
  {
    RuleFor(x => x.CashSaleNumber)
    .MinimumLength(2)
    .MaximumLength(30);

    RuleFor(x => x.HsCode)
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.HsName)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ItemCode)
         .MinimumLength(2)
         .MaximumLength(16);

    RuleFor(x => x.ItemName)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.SaleID)
         .NotEmpty()
         .WithMessage("Sale ID is required.");

    RuleFor(x => x.UnitOfMeasure)
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.VATClass)
         .MinimumLength(0)
         .MaximumLength(4);
  }
}
