using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetComputerAssetsWriteOffValidator : Validator<GetComputerAssetsWriteOffByIdRequest>
{
  public GetComputerAssetsWriteOffValidator()
  {
    RuleFor(x => x.WriteOffID)
      .NotEmpty()
      .WithMessage("The write off id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
