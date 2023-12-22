using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CommandDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetCommandDetailValidator : Validator<GetCommandDetailByIdRequest>
{
  public GetCommandDetailValidator()
  {
    RuleFor(x => x.CommandId)
      .NotEmpty()
      .WithMessage("The command id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
