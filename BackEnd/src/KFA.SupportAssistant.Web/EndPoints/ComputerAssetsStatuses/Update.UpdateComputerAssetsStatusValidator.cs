using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsStatuses;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateComputerAssetsStatusValidator : Validator<UpdateComputerAssetsStatusRequest>
{
  public UpdateComputerAssetsStatusValidator()
  {
     RuleFor(x => x.AssetsStatusID)
     .NotEmpty()
     .WithMessage("Assets Status ID is required.");

RuleFor(x => x.Description)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.DoneBy)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.Status)
     .MinimumLength(2)
     .MaximumLength(255);             

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.AssetsStatusID)
      .Must((args, id) => checkIds(args.AssetsStatusID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
