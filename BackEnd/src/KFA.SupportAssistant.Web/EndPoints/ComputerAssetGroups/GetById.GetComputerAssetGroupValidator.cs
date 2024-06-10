using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetComputerAssetGroupValidator : Validator<GetComputerAssetGroupByIdRequest>
{
  public GetComputerAssetGroupValidator()
  {
    RuleFor(x => x.GroupID)
      .NotEmpty()
      .WithMessage("The group id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
