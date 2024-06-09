using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsStatuses;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateComputerAssetsStatusValidator : Validator<CreateComputerAssetsStatusRequest>
{
  public CreateComputerAssetsStatusValidator()
  {
     RuleFor(x => x.AssetsStatusID)
     .NotEmpty()
     .WithMessage("Assets Status ID is required.");

RuleFor(x => x.Description)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.DoneBy)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.Status)
     .MinimumLength(2)
     .MaximumLength(255);             
  }
}