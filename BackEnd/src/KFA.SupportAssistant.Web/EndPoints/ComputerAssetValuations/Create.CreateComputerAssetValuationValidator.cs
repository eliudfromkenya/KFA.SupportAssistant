using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetValuations;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateComputerAssetValuationValidator : Validator<CreateComputerAssetValuationRequest>
{
  public CreateComputerAssetValuationValidator()
  {
     RuleFor(x => x.AssetDetailID)
     .NotEmpty()
     .WithMessage("Asset Detail ID is required.");

RuleFor(x => x.Description)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.RevaluationID)
     .NotEmpty()
     .WithMessage("Revaluation ID is required.");

RuleFor(x => x.Status)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.Value)
     .NotEmpty()
     .WithMessage("Value is required.");             
  }
}