using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceAccessories;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetComputerAssetsMaintainceAccessoryValidator : Validator<GetComputerAssetsMaintainceAccessoryByIdRequest>
{
  public GetComputerAssetsMaintainceAccessoryValidator()
  {
    RuleFor(x => x.AccessoryID)
      .NotEmpty()
      .WithMessage("The accessory id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
