using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchComputerAssetsWriteOffValidator : Validator<PatchComputerAssetsWriteOffRequest>
{
  public PatchComputerAssetsWriteOffValidator()
  {
    RuleFor(x => x.WriteOffID)
     .NotEmpty()
     .WithMessage("The write off id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}

