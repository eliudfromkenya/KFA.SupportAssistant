using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateComputerAssetsWriteOffValidator : Validator<UpdateComputerAssetsWriteOffRequest>
{
  public UpdateComputerAssetsWriteOffValidator()
  {
    RuleFor(x => x.Description)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.ReasonForWriteOff)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.WriteOffID)
         .NotEmpty()
         .WithMessage("Write Off ID is required.");

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.WriteOffID)
      .Must((args, id) => checkIds(args.WriteOffID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
