using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteItemGroupValidator : Validator<DeleteItemGroupRequest>
{
  public DeleteItemGroupValidator()
  {
    RuleFor(x => x.GroupId)
      .NotEmpty()
      .WithMessage("The group id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
