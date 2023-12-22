using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.Suppliers;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetSupplierValidator : Validator<GetSupplierByIdRequest>
{
  public GetSupplierValidator()
  {
    RuleFor(x => x.SupplierId)
      .NotEmpty()
      .WithMessage("The supplier id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
