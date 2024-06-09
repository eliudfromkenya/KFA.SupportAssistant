using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerAssetsMaintainceReceivingValidator : Validator<DeleteComputerAssetsMaintainceReceivingRequest>
{
  public DeleteComputerAssetsMaintainceReceivingValidator()
  {
    RuleFor(x => x.ReceiveID)
      .NotEmpty()
      .WithMessage("The receive id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}

