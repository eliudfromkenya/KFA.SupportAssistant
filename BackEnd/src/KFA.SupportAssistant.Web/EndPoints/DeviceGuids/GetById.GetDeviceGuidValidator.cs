using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DeviceGuids;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetDeviceGuidValidator : Validator<GetDeviceGuidByIdRequest>
{
  public GetDeviceGuidValidator()
  {
    RuleFor(x => x.Guid)
      .NotEmpty()
      .WithMessage("The guid to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
