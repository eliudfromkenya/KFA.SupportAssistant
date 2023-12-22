using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.Suppliers;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchSupplierValidator : Validator<PatchSupplierRequest>
{
  public PatchSupplierValidator()
  {
    RuleFor(x => x.SupplierId)
     .NotEmpty()
     .WithMessage("The supplier id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
