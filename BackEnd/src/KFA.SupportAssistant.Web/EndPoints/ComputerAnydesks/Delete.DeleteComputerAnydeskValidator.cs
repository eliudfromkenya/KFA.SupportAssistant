using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAnydesks;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerAnydeskValidator : Validator<DeleteComputerAnydeskRequest>
{
  public DeleteComputerAnydeskValidator()
  {
    RuleFor(x => x.AnyDeskId)
      .NotEmpty()
      .WithMessage("The anydesk id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
