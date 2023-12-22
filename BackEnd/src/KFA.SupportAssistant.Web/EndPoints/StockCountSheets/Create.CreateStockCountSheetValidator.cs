using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StockCountSheets;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateStockCountSheetValidator : Validator<CreateStockCountSheetRequest>
{
  public CreateStockCountSheetValidator()
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
  }
}
