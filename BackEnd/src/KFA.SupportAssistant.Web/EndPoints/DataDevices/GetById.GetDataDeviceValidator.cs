using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DataDevices;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetDataDeviceValidator : Validator<GetDataDeviceByIdRequest>
{
  public GetDataDeviceValidator()
  {
    RuleFor(x => x.DeviceId)
      .NotEmpty()
      .WithMessage("The device id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
