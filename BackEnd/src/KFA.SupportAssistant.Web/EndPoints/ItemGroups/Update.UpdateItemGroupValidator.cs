using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateItemGroupValidator : Validator<UpdateItemGroupRequest>
{
  public UpdateItemGroupValidator()
  {
    RuleFor(x => x.GroupId)
    .NotEmpty()
    .WithMessage("Group Id is required.");

    RuleFor(x => x.Name)
         .NotEmpty()
         .WithMessage("Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ParentGroupId)
         .MinimumLength(2)
         .MaximumLength(255);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.GroupId)
      .Must((args, id) => checkIds(args.GroupId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
