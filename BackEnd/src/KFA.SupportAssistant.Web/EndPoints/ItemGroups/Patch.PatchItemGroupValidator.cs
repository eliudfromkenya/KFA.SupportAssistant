using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchItemGroupValidator : Validator<PatchItemGroupRequest>
{
  public PatchItemGroupValidator()
  {
    RuleFor(x => x.GroupId)
     .NotEmpty()
     .WithMessage("The group id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
