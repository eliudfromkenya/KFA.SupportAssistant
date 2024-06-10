using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateComputerAssetsMaintainceDispatchValidator : Validator<UpdateComputerAssetsMaintainceDispatchRequest>
{
  public UpdateComputerAssetsMaintainceDispatchValidator()
  {
    RuleFor(x => x.CollectedBy)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.Description)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.DispatchID)
         .NotEmpty()
         .WithMessage("Dispatch ID is required.");

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.ReasonToSend)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.SentBy)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ToDepartmentCode)
         .MinimumLength(2)
         .MaximumLength(255);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.DispatchID)
      .Must((args, id) => checkIds(args.DispatchID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
