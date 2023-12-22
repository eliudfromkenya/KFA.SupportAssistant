using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteQRRequestItemValidator : Validator<DeleteQRRequestItemRequest>
{
  public DeleteQRRequestItemValidator()
  {
    RuleFor(x => x.SaleID)
      .NotEmpty()
      .WithMessage("The sale id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
