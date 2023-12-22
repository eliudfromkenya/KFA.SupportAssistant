using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateQRRequestItemValidator : Validator<UpdateQRRequestItemRequest>
{
  public UpdateQRRequestItemValidator()
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

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.SaleID)
      .Must((args, id) => checkIds(args.SaleID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
