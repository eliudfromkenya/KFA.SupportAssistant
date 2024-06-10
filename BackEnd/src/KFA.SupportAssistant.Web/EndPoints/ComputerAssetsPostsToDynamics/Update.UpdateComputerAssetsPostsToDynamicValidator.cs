using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateComputerAssetsPostsToDynamicValidator : Validator<UpdateComputerAssetsPostsToDynamicRequest>
{
  public UpdateComputerAssetsPostsToDynamicValidator()
  {
    RuleFor(x => x.Description)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.PostID)
         .NotEmpty()
         .WithMessage("Post ID is required.");

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.PostID)
      .Must((args, id) => checkIds(args.PostID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
