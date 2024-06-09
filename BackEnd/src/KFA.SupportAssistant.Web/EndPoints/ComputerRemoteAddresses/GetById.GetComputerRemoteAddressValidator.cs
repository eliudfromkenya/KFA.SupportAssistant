using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerRemoteAddresses;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetComputerRemoteAddressValidator : Validator<GetComputerRemoteAddressByIdRequest>
{
  public GetComputerRemoteAddressValidator()
  {
    RuleFor(x => x.AnyDeskId)
      .NotEmpty()
      .WithMessage("The anydesk id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
