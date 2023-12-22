using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.VendorCodesRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchVendorCodesRequestValidator : Validator<PatchVendorCodesRequestRequest>
{
  public PatchVendorCodesRequestValidator()
  {
    RuleFor(x => x.VendorCodeRequestID)
     .NotEmpty()
     .WithMessage("The vendor code request id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
