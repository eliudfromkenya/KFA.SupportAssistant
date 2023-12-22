using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CommandDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchCommandDetailValidator : Validator<PatchCommandDetailRequest>
{
  public PatchCommandDetailValidator()
  {
    RuleFor(x => x.CommandId)
     .NotEmpty()
     .WithMessage("The command id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
