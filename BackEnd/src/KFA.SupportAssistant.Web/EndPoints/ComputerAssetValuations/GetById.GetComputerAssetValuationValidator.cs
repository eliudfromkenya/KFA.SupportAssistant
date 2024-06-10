using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetValuations;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetComputerAssetValuationValidator : Validator<GetComputerAssetValuationByIdRequest>
{
  public GetComputerAssetValuationValidator()
  {
    RuleFor(x => x.RevaluationID)
      .NotEmpty()
      .WithMessage("The revaluation id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
