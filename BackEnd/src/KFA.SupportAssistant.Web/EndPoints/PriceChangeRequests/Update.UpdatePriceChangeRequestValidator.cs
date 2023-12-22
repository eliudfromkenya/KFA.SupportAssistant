using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PriceChangeRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdatePriceChangeRequestValidator : Validator<UpdatePriceChangeRequestRequest>
{
  public UpdatePriceChangeRequestValidator()
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

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.RequestID)
      .Must((args, id) => checkIds(args.RequestID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
