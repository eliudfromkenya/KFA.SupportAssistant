using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetMaintainces;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetComputerAssetMaintainceValidator : Validator<GetComputerAssetMaintainceByIdRequest>
{
  public GetComputerAssetMaintainceValidator()
  {
    RuleFor(x => x.MaintainceID)
      .NotEmpty()
      .WithMessage("The maintaince id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
