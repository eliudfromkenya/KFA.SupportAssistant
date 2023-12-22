using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DataDevices;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateDataDeviceValidator : Validator<UpdateDataDeviceRequest>
{
  public UpdateDataDeviceValidator()
  {
    RuleFor(x => x.DeviceCaption)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.DeviceCode)
         .NotEmpty()
         .WithMessage("Device Code is required.")
         .MinimumLength(2)
         .MaximumLength(100);

    RuleFor(x => x.DeviceId)
         .NotEmpty()
         .WithMessage("Device Id is required.");

    RuleFor(x => x.DeviceName)
         .NotEmpty()
         .WithMessage("Device Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.DeviceNumber)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.DeviceRight)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.StationID)
         .NotEmpty()
         .WithMessage("Station is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.TypeOfDevice)
         .MinimumLength(2)
         .MaximumLength(255);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.DeviceId)
      .Must((args, id) => checkIds(args.DeviceId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
