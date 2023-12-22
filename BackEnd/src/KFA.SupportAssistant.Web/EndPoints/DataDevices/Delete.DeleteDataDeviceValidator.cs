using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DataDevices;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteDataDeviceValidator : Validator<DeleteDataDeviceRequest>
{
  public DeleteDataDeviceValidator()
  {
    RuleFor(x => x.DeviceId)
      .NotEmpty()
      .WithMessage("The device id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
