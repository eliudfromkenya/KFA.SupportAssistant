using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetComputerAssetAcquisitionValidator : Validator<GetComputerAssetAcquisitionByIdRequest>
{
  public GetComputerAssetAcquisitionValidator()
  {
    RuleFor(x => x.AcquisitionID)
      .NotEmpty()
      .WithMessage("The acquisition id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
