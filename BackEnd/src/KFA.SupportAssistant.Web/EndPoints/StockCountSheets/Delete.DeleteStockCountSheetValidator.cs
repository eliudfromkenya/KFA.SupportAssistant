using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StockCountSheets;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteStockCountSheetValidator : Validator<DeleteStockCountSheetRequest>
{
  public DeleteStockCountSheetValidator()
  {
    RuleFor(x => x.CountSheetId)
      .NotEmpty()
      .WithMessage("The count sheet id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
