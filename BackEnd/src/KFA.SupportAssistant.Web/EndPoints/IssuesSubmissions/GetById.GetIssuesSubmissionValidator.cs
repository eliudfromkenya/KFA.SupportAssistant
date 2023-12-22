using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesSubmissions;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetIssuesSubmissionValidator : Validator<GetIssuesSubmissionByIdRequest>
{
  public GetIssuesSubmissionValidator()
  {
    RuleFor(x => x.SubmissionID)
      .NotEmpty()
      .WithMessage("The submission id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
