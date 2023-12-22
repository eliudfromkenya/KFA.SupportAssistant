using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateStaffGroupValidator : Validator<UpdateStaffGroupRequest>
{
  public UpdateStaffGroupValidator()
  {
    RuleFor(x => x.Description)
    .NotEmpty()
    .WithMessage("Description is required.")
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.GroupNumber)
         .NotEmpty()
         .WithMessage("Group Number is required.");

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.GroupNumber)
      .Must((args, id) => checkIds(args.GroupNumber, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
