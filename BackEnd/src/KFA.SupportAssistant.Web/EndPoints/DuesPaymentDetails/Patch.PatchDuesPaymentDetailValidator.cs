using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DuesPaymentDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchDuesPaymentDetailValidator : Validator<PatchDuesPaymentDetailRequest>
{
  public PatchDuesPaymentDetailValidator()
  {
    RuleFor(x => x.PaymentID)
     .NotEmpty()
     .WithMessage("The payment id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
