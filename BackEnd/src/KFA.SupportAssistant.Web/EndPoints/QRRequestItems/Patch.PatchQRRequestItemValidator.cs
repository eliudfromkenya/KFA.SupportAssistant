using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchQRRequestItemValidator : Validator<PatchQRRequestItemRequest>
{
  public PatchQRRequestItemValidator()
  {
    RuleFor(x => x.SaleID)
     .NotEmpty()
     .WithMessage("The sale id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
