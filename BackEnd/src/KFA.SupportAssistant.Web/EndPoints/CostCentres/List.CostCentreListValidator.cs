namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CostCentreListValidator : Validator<CostCentreListRequest>
{
  public CostCentreListValidator()
  {
    //RuleFor(x => x.Description)
    // .NotEmpty()
    // .WithMessage("Name is required.")
    // .MinimumLength(2)
    // .MaximumLength(30);

    //RuleFor(x => x.Skip)
    // .NotEmpty()
    // .WithMessage("Number of cost centres to skip is required.");

    //RuleFor(x => x.Count)
    // .NotEmpty()
    // .WithMessage("Maximum number of cost centres to return is required.");

    //RuleFor(x => x.Description)
    //.NotEmpty()
    //.WithMessage("Name is required.")
    //.MinimumLength(2)
    //.MaximumLength(30);
    //.MaximumLength(30);
  }
}
