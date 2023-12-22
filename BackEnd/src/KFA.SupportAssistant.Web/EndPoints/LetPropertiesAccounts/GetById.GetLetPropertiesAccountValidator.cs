using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.LetPropertiesAccounts;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetLetPropertiesAccountValidator : Validator<GetLetPropertiesAccountByIdRequest>
{
  public GetLetPropertiesAccountValidator()
  {
    RuleFor(x => x.LetPropertyAccountId)
      .NotEmpty()
      .WithMessage("The let property account id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
