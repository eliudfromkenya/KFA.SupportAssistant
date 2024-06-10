using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetVendorValidator : Validator<GetVendorByIdRequest>
{
  public GetVendorValidator()
  {
    RuleFor(x => x.VendorCode)
      .NotEmpty()
      .WithMessage("The vendor code to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
