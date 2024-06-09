using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteVendorValidator : Validator<DeleteVendorRequest>
{
  public DeleteVendorValidator()
  {
    RuleFor(x => x.VendorCode)
      .NotEmpty()
      .WithMessage("The vendor code to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}

