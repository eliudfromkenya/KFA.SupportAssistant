using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetQRRequestItemValidator : Validator<GetQRRequestItemByIdRequest>
{
  public GetQRRequestItemValidator()
  {
    RuleFor(x => x.SaleID)
      .NotEmpty()
      .WithMessage("The sale id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
