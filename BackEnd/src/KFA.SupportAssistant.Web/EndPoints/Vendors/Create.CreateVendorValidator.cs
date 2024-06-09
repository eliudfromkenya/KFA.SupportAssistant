using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateVendorValidator : Validator<CreateVendorRequest>
{
  public CreateVendorValidator()
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
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.VendorCode)
     .NotEmpty()
     .WithMessage("Vendor Code is required.");             
  }
}
