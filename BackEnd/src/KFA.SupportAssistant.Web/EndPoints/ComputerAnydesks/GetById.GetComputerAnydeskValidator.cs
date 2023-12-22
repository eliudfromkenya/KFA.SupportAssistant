using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAnydesks;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetComputerAnydeskValidator : Validator<GetComputerAnydeskByIdRequest>
{
  public GetComputerAnydeskValidator()
  {
    RuleFor(x => x.AnyDeskId)
      .NotEmpty()
      .WithMessage("The anydesk id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
