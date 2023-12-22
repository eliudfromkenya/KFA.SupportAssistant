using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StockItemCodesRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateStockItemCodesRequestValidator : Validator<CreateStockItemCodesRequestRequest>
{
  public CreateStockItemCodesRequestValidator()
  {
    RuleFor(x => x.AttandedBy)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.CostCentreCode)
         .NotEmpty()
         .WithMessage("Cost Centre Code is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.CostPrice)
         .NotEmpty()
         .WithMessage("Cost Price is required.");

    RuleFor(x => x.Description)
         .NotEmpty()
         .WithMessage("Description is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Distributor)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ItemCode)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ItemCodeRequestID)
         .NotEmpty()
         .WithMessage("Item Code Request ID is required.");

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.RequestingUser)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.SellingPrice)
         .NotEmpty()
         .WithMessage("Selling Price is required.");

    RuleFor(x => x.Status)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Supplier)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.TimeAttended)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.TimeOfRequest)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.UnitOfMeasure)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
