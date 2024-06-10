using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateComputerAssetsMaintainceReceivingValidator : Validator<CreateComputerAssetsMaintainceReceivingRequest>
{
  public CreateComputerAssetsMaintainceReceivingValidator()
  {
    RuleFor(x => x.BroughtBy)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.Description)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.FromDepartmentCode)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.ReasonToSend)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ReceiveID)
         .NotEmpty()
         .WithMessage("Receive ID is required.");

    RuleFor(x => x.RecievedBy)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
