using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StockCountSheets;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchStockCountSheetValidator : Validator<PatchStockCountSheetRequest>
{
  public PatchStockCountSheetValidator()
  {
    RuleFor(x => x.CountSheetId)
     .NotEmpty()
     .WithMessage("The count sheet id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
