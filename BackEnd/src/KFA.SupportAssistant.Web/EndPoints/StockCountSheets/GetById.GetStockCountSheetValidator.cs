using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StockCountSheets;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetStockCountSheetValidator : Validator<GetStockCountSheetByIdRequest>
{
  public GetStockCountSheetValidator()
  {
    RuleFor(x => x.CountSheetId)
      .NotEmpty()
      .WithMessage("The count sheet id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
