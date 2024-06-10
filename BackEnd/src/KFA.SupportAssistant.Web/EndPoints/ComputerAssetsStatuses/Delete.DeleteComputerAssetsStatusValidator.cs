using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsStatuses;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerAssetsStatusValidator : Validator<DeleteComputerAssetsStatusRequest>
{
  public DeleteComputerAssetsStatusValidator()
  {
    RuleFor(x => x.AssetsStatusID)
      .NotEmpty()
      .WithMessage("The assets status id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
