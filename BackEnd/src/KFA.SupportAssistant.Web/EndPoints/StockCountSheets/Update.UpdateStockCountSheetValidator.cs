using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StockCountSheets;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateStockCountSheetValidator : Validator<UpdateStockCountSheetRequest>
{
  public UpdateStockCountSheetValidator()
  {
    RuleFor(x => x.CountSheetDocumentId)
    .NotEmpty()
    .WithMessage("Count Sheet Document Id is required.");

    RuleFor(x => x.CountSheetId)
         .NotEmpty()
         .WithMessage("Count Sheet Id is required.");

    RuleFor(x => x.DocumentNumber)
         .MinimumLength(2)
         .MaximumLength(10);

    RuleFor(x => x.ItemCode)
         .NotEmpty()
         .WithMessage("Item Code is required.");

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.CountSheetId)
      .Must((args, id) => checkIds(args.CountSheetId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
