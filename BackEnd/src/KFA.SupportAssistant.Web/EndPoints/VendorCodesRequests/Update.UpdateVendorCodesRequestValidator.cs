using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.VendorCodesRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateVendorCodesRequestValidator : Validator<UpdateVendorCodesRequestRequest>
{
  public UpdateVendorCodesRequestValidator()
  {
    RuleFor(x => x.AttandedBy)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.CostCentreCode)
         .NotEmpty()
         .WithMessage("Cost Centre Code is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Description)
         .NotEmpty()
         .WithMessage("Description is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.RequestingUser)
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

    RuleFor(x => x.VendorCode)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.VendorCodeRequestID)
         .NotEmpty()
         .WithMessage("Vendor Code Request ID is required.");

    RuleFor(x => x.VendorType)
         .MinimumLength(2)
         .MaximumLength(255);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.VendorCodeRequestID)
      .Must((args, id) => checkIds(args.VendorCodeRequestID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
