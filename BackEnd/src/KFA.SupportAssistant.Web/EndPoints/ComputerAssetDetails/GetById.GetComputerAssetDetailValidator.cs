using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetComputerAssetDetailValidator : Validator<GetComputerAssetDetailByIdRequest>
{
  public GetComputerAssetDetailValidator()
  {
    RuleFor(x => x.AssetID)
      .NotEmpty()
      .WithMessage("The asset id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
