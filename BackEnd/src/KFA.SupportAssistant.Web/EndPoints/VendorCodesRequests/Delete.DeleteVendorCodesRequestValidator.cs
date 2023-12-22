using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.VendorCodesRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteVendorCodesRequestValidator : Validator<DeleteVendorCodesRequestRequest>
{
  public DeleteVendorCodesRequestValidator()
  {
    RuleFor(x => x.VendorCodeRequestID)
      .NotEmpty()
      .WithMessage("The vendor code request id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
