using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetMaintainces;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateComputerAssetMaintainceValidator : Validator<UpdateComputerAssetMaintainceRequest>
{
  public UpdateComputerAssetMaintainceValidator()
  {
     RuleFor(x => x.AssetState)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.Description)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.Diagnosis)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.DoneBy)
     .MinimumLength(2)
     .MaximumLength(255);

RuleFor(x => x.MaintainceID)
     .NotEmpty()
     .WithMessage("Maintaince ID is required.");

RuleFor(x => x.Narration)
     .MinimumLength(2)
     .MaximumLength(500);

RuleFor(x => x.WhatWasDone)
     .MinimumLength(2)
     .MaximumLength(255);             

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.MaintainceID)
      .Must((args, id) => checkIds(args.MaintainceID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
