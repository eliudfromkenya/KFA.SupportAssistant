using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.LeasedPropertiesAccounts;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetLeasedPropertiesAccountValidator : Validator<GetLeasedPropertiesAccountByIdRequest>
{
  public GetLeasedPropertiesAccountValidator()
  {
    RuleFor(x => x.LeasedPropertyAccountId)
      .NotEmpty()
      .WithMessage("The leased property account id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
