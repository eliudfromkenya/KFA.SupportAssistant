using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.DeviceGuids;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchDeviceGuidValidator : Validator<PatchDeviceGuidRequest>
{
  public PatchDeviceGuidValidator()
  {
    RuleFor(x => x.Guid)
     .NotEmpty()
     .WithMessage("The guid of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
