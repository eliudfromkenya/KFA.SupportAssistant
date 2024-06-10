using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerRemoteAddresses;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerRemoteAddressValidator : Validator<DeleteComputerRemoteAddressRequest>
{
  public DeleteComputerRemoteAddressValidator()
  {
    RuleFor(x => x.AnyDeskId)
      .NotEmpty()
      .WithMessage("The anydesk id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
