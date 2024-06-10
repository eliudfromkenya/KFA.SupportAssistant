using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerAssetsWriteOffValidator : Validator<DeleteComputerAssetsWriteOffRequest>
{
  public DeleteComputerAssetsWriteOffValidator()
  {
    RuleFor(x => x.WriteOffID)
      .NotEmpty()
      .WithMessage("The write off id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
