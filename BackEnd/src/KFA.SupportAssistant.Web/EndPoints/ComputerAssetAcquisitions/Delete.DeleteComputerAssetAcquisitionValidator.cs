using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerAssetAcquisitionValidator : Validator<DeleteComputerAssetAcquisitionRequest>
{
  public DeleteComputerAssetAcquisitionValidator()
  {
    RuleFor(x => x.AcquisitionID)
      .NotEmpty()
      .WithMessage("The acquisition id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
