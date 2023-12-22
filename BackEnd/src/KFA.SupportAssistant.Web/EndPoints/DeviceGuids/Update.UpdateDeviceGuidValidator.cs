using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DeviceGuids;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateDeviceGuidValidator : Validator<UpdateDeviceGuidRequest>
{
  public UpdateDeviceGuidValidator()
  {
    RuleFor(x => x.Guid)
    .NotEmpty()
    .WithMessage("Guid is required.")
    .MinimumLength(2)
    .MaximumLength(100);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.Guid)
      .Must((args, id) => checkIds(args.Guid, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
