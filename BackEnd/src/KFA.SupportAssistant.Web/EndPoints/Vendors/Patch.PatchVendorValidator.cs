using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchVendorValidator : Validator<PatchVendorRequest>
{
  public PatchVendorValidator()
  {
    RuleFor(x => x.VendorCode)
     .NotEmpty()
     .WithMessage("The vendor code of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}

