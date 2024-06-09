using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateComputerAssetGroupValidator : Validator<UpdateComputerAssetGroupRequest>
{
  public UpdateComputerAssetGroupValidator()
  {
   RuleFor(x => x.Description)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.GroupID)
     .NotEmpty()
     .WithMessage("Group ID is required.");

RuleFor(x => x.GroupName)
     .NotEmpty()
     .WithMessage("Group Name is required.")
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.LastAssignedValue)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.Prefix)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.Suffix)
     .MinimumLength(2)
     .MaximumLength(255);             

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.GroupID)
      .Must((args, id) => checkIds(args.GroupID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
