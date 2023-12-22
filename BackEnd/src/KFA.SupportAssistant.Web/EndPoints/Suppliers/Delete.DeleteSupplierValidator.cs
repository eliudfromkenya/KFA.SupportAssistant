using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.Suppliers;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteSupplierValidator : Validator<DeleteSupplierRequest>
{
  public DeleteSupplierValidator()
  {
    RuleFor(x => x.SupplierId)
      .NotEmpty()
      .WithMessage("The supplier id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
