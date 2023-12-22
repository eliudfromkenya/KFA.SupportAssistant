using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PriceChangeRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeletePriceChangeRequestValidator : Validator<DeletePriceChangeRequestRequest>
{
  public DeletePriceChangeRequestValidator()
  {
    RuleFor(x => x.RequestID)
      .NotEmpty()
      .WithMessage("The request id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
