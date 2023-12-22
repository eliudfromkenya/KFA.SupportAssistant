using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PriceChangeRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetPriceChangeRequestValidator : Validator<GetPriceChangeRequestByIdRequest>
{
  public GetPriceChangeRequestValidator()
  {
    RuleFor(x => x.RequestID)
      .NotEmpty()
      .WithMessage("The request id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
