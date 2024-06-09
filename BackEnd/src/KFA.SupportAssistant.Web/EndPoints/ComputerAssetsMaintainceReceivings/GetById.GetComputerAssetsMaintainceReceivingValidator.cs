using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetComputerAssetsMaintainceReceivingValidator : Validator<GetComputerAssetsMaintainceReceivingByIdRequest>
{
  public GetComputerAssetsMaintainceReceivingValidator()
  {
    RuleFor(x => x.ReceiveID)
      .NotEmpty()
      .WithMessage("The receive id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
