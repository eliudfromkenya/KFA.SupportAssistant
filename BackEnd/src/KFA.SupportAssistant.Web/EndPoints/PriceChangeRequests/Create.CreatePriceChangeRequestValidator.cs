using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PriceChangeRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreatePriceChangeRequestValidator : Validator<CreatePriceChangeRequestRequest>
{
  public CreatePriceChangeRequestValidator()
  {
    RuleFor(x => x.AttandedBy)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.BatchNumber)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.CostCentreCode)
         .NotEmpty()
         .WithMessage("Cost Centre Code is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.CostPrice)
         .NotEmpty()
         .WithMessage("Cost Price is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ItemCode)
         .NotEmpty()
         .WithMessage("Item Code is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.RequestID)
         .NotEmpty()
         .WithMessage("Request ID is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.RequestingUser)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.SellingPrice)
         .NotEmpty()
         .WithMessage("Selling Price is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Status)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.TimeAttended)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.TimeOfRequest)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
