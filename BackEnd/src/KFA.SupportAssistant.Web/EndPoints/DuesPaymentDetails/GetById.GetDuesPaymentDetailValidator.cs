using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DuesPaymentDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetDuesPaymentDetailValidator : Validator<GetDuesPaymentDetailByIdRequest>
{
  public GetDuesPaymentDetailValidator()
  {
    RuleFor(x => x.PaymentID)
      .NotEmpty()
      .WithMessage("The payment id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
