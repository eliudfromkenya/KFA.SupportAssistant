using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.LedgerAccounts;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateLedgerAccountValidator : Validator<CreateLedgerAccountRequest>
{
  public CreateLedgerAccountValidator()
  {
    RuleFor(x => x.CostCentreCode)
    .MinimumLength(2)
    .MaximumLength(25);

    RuleFor(x => x.Description)
         .NotEmpty()
         .WithMessage("Description is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.GroupName)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.LedgerAccountCode)
         .NotEmpty()
         .WithMessage("Ledger Account Code is required.")
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.LedgerAccountId)
         .NotEmpty()
         .WithMessage("Ledger Account Id is required.")
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.MainGroup)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);
  }
}
