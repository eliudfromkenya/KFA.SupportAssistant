using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DuesPaymentDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateDuesPaymentDetailValidator : Validator<CreateDuesPaymentDetailRequest>
{
  public CreateDuesPaymentDetailValidator()
  {
    RuleFor(x => x.Amount)
    .NotEmpty()
    .WithMessage("Amount is required.");

    RuleFor(x => x.Date)
         .NotEmpty()
         .WithMessage("Date is required.");

    RuleFor(x => x.DocumentNo)
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.EmployeeID)
         .NotEmpty()
         .WithMessage("Employee ID is required.");

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.OpeningBalance)
         .NotEmpty()
         .WithMessage("Opening Balance is required.");

    RuleFor(x => x.PaymentID)
         .NotEmpty()
         .WithMessage("Payment ID is required.");

    RuleFor(x => x.PaymentType)
         .NotEmpty()
         .WithMessage("Payment Type is required.")
         .MinimumLength(2)
         .MaximumLength(10);

    RuleFor(x => x.ProcessedBy)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
