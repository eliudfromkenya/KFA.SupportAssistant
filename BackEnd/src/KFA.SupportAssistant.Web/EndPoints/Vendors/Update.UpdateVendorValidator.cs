using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateVendorValidator : Validator<UpdateVendorRequest>
{
  public UpdateVendorValidator()
  {
     RuleFor(x => x.Contact)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.Descriptions)
     .NotEmpty()
     .WithMessage("Descriptions is required.")
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.Email)
     .NotEmpty()
     .WithMessage("Email is required.")
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.VendorCode)
     .NotEmpty()
     .WithMessage("Vendor Code is required.");             

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.VendorCode)
      .Must((args, id) => checkIds(args.VendorCode, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
