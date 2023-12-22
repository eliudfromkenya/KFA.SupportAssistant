using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetBatchHeaders;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdatePurchasesBudgetBatchHeaderValidator : Validator<UpdatePurchasesBudgetBatchHeaderRequest>
{
  public UpdatePurchasesBudgetBatchHeaderValidator()
  {
    RuleFor(x => x.ApprovedBy)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.BatchKey)
         .NotEmpty()
         .WithMessage("Batch Key is required.");

    RuleFor(x => x.BatchNumber)
         .MinimumLength(2)
         .MaximumLength(10);

    RuleFor(x => x.CostCentreCode)
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.MonthFrom)
         .NotEmpty()
         .WithMessage("Month From is required.")
         .MinimumLength(2)
         .MaximumLength(10);

    RuleFor(x => x.MonthTo)
         .NotEmpty()
         .WithMessage("Month To is required.")
         .MinimumLength(2)
         .MaximumLength(10);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.PreparedBy)
         .MinimumLength(2)
         .MaximumLength(255);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.BatchKey)
      .Must((args, id) => checkIds(args.BatchKey, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
