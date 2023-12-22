using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DeviceGuids;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateDeviceGuidValidator : Validator<CreateDeviceGuidRequest>
{
  public CreateDeviceGuidValidator()
  {
    RuleFor(x => x.Guid)
    .NotEmpty()
    .WithMessage("Guid is required.")
    .MinimumLength(2)
    .MaximumLength(100);
  }
}
