using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdatePurchasesBudgetDetailValidator : Validator<UpdatePurchasesBudgetDetailRequest>
{
  public UpdatePurchasesBudgetDetailValidator()
  {
    RuleFor(x => x.ItemCode)
    .MinimumLength(2)
    .MaximumLength(15);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.PurchasesBudgetDetailId)
         .NotEmpty()
         .WithMessage("Purchases Budget Detail Id is required.");

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.PurchasesBudgetDetailId)
      .Must((args, id) => checkIds(args.PurchasesBudgetDetailId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
