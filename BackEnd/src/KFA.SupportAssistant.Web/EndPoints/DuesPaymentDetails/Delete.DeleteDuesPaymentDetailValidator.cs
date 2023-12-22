using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DuesPaymentDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteDuesPaymentDetailValidator : Validator<DeleteDuesPaymentDetailRequest>
{
  public DeleteDuesPaymentDetailValidator()
  {
    RuleFor(x => x.PaymentID)
      .NotEmpty()
      .WithMessage("The payment id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
