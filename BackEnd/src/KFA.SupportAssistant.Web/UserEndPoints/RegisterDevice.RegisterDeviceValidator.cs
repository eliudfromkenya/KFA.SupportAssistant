using FluentValidation;
using KFA.SupportAssistant.Infrastructure.Data.Config;

namespace KFA.SupportAssistant.Web.UserEndPoints;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class RegisterDeviceValidator : Validator<RegisterDeviceRequest>
{
  public RegisterDeviceValidator()
  {
    RuleFor(x => x.Description)
      .NotEmpty()
      .WithMessage("Description is required.")
      .MinimumLength(2)
      .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    RuleFor(x => x.Description)
    .NotEmpty()
    .WithMessage("Device caption is required.")
    .MinimumLength(2)
    .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    RuleFor(x => x.DeviceCode)
  .NotEmpty()
  .WithMessage("Device Code is required.")
  .MinimumLength(2)
  .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);
  }
}
