using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAnydesks;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchComputerAnydeskValidator : Validator<PatchComputerAnydeskRequest>
{
  public PatchComputerAnydeskValidator()
  {
    RuleFor(x => x.AnyDeskId)
     .NotEmpty()
     .WithMessage("The anydesk id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
