using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.Suppliers;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateSupplierValidator : Validator<CreateSupplierRequest>
{
  public CreateSupplierValidator()
  {
    RuleFor(x => x.Address)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.ContactPerson)
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.CostCentreCode)
         .MinimumLength(2)
         .MaximumLength(10);

    RuleFor(x => x.Description)
         .NotEmpty()
         .WithMessage("Description is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Email)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.PostalCode)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.SupplierCode)
         .NotEmpty()
         .WithMessage("Supplier Code is required.")
         .MinimumLength(2)
         .MaximumLength(10);

    RuleFor(x => x.SupplierId)
         .NotEmpty()
         .WithMessage("Supplier Id is required.")
         .MinimumLength(2)
         .MaximumLength(10);

    RuleFor(x => x.Telephone)
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.Town)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
