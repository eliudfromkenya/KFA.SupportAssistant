using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.SystemUsers;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateSystemUserValidator : Validator<CreateSystemUserRequest>
{
  public CreateSystemUserValidator()
  {
    RuleFor(x => x.Contact)
    .NotEmpty()
    .WithMessage("Contact is required.")
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.EmailAddress)
         .NotEmpty()
         .WithMessage("Email Address is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ExpirationDate)
         .NotEmpty()
         .WithMessage("Expiration Date is required.");

    RuleFor(x => x.MaturityDate)
         .NotEmpty()
         .WithMessage("Maturity Date is required.");

    RuleFor(x => x.NameOfTheUser)
         .NotEmpty()
         .WithMessage("Name Of The User is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.PasswordHash)
         .NotEmpty()
         .WithMessage("Password Hash is required.");

    RuleFor(x => x.PasswordSalt)
         .NotEmpty()
         .WithMessage("Password Salt is required.");

    RuleFor(x => x.RoleId)
         .NotEmpty()
         .WithMessage("Role Id is required.");

    RuleFor(x => x.UserId)
         .NotEmpty()
         .WithMessage("User Id is required.");

    RuleFor(x => x.Username)
         .NotEmpty()
         .WithMessage("Username is required.")
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
