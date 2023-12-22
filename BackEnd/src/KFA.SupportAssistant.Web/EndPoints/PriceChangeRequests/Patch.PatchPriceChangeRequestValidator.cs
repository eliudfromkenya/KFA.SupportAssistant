using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PriceChangeRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchPriceChangeRequestValidator : Validator<PatchPriceChangeRequestRequest>
{
  public PatchPriceChangeRequestValidator()
  {
    RuleFor(x => x.RequestID)
     .NotEmpty()
     .WithMessage("The request id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
