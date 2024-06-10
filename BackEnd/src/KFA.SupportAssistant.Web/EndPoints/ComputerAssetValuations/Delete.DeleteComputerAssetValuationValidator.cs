using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetValuations;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerAssetValuationValidator : Validator<DeleteComputerAssetValuationRequest>
{
  public DeleteComputerAssetValuationValidator()
  {
    RuleFor(x => x.RevaluationID)
      .NotEmpty()
      .WithMessage("The revaluation id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
