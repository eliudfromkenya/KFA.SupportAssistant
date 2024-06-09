using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsStatuses;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchComputerAssetsStatusValidator : Validator<PatchComputerAssetsStatusRequest>
{
  public PatchComputerAssetsStatusValidator()
  {
    RuleFor(x => x.AssetsStatusID)
     .NotEmpty()
     .WithMessage("The assets status id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}

