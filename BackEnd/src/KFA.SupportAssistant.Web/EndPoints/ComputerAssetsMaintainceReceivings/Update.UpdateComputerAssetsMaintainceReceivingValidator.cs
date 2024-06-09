using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateComputerAssetsMaintainceReceivingValidator : Validator<UpdateComputerAssetsMaintainceReceivingRequest>
{
  public UpdateComputerAssetsMaintainceReceivingValidator()
  {
     RuleFor(x => x.BroughtBy)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.Description)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.FromDepartmentCode)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.Narration)
     .MinimumLength(2)
     .MaximumLength(500);

RuleFor(x => x.ReasonToSend)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.ReceiveID)
     .NotEmpty()
     .WithMessage("Receive ID is required.");

RuleFor(x => x.RecievedBy)
     .MinimumLength(2)
     .MaximumLength(255);             

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.ReceiveID)
      .Must((args, id) => checkIds(args.ReceiveID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
