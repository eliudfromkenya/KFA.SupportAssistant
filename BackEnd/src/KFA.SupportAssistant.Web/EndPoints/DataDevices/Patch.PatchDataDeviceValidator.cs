using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DataDevices;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchDataDeviceValidator : Validator<PatchDataDeviceRequest>
{
  public PatchDataDeviceValidator()
  {
    RuleFor(x => x.DeviceId)
     .NotEmpty()
     .WithMessage("The device id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
