using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceAccessories;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerAssetsMaintainceAccessoryValidator : Validator<DeleteComputerAssetsMaintainceAccessoryRequest>
{
  public DeleteComputerAssetsMaintainceAccessoryValidator()
  {
    RuleFor(x => x.AccessoryID)
      .NotEmpty()
      .WithMessage("The accessory id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}

