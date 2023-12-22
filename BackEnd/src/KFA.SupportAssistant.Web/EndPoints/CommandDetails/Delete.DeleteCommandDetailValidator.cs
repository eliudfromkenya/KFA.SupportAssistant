using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CommandDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteCommandDetailValidator : Validator<DeleteCommandDetailRequest>
{
  public DeleteCommandDetailValidator()
  {
    RuleFor(x => x.CommandId)
      .NotEmpty()
      .WithMessage("The command id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
