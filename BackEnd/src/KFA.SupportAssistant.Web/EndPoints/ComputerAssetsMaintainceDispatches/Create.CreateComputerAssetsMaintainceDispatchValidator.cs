using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateComputerAssetsMaintainceDispatchValidator : Validator<CreateComputerAssetsMaintainceDispatchRequest>
{
  public CreateComputerAssetsMaintainceDispatchValidator()
  {
    RuleFor(x => x.CollectedBy)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.Description)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.DispatchID)
         .NotEmpty()
         .WithMessage("Dispatch ID is required.");

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.ReasonToSend)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.SentBy)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ToDepartmentCode)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
