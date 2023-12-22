using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetItemGroupValidator : Validator<GetItemGroupByIdRequest>
{
  public GetItemGroupValidator()
  {
    RuleFor(x => x.GroupId)
      .NotEmpty()
      .WithMessage("The group id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
