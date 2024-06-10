using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerAssetDetailValidator : Validator<DeleteComputerAssetDetailRequest>
{
  public DeleteComputerAssetDetailValidator()
  {
    RuleFor(x => x.AssetID)
      .NotEmpty()
      .WithMessage("The asset id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
