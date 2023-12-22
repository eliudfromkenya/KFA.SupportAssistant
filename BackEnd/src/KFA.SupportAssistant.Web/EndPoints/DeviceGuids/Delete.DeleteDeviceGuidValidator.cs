using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DeviceGuids;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteDeviceGuidValidator : Validator<DeleteDeviceGuidRequest>
{
  public DeleteDeviceGuidValidator()
  {
    RuleFor(x => x.Guid)
      .NotEmpty()
      .WithMessage("The guid to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
