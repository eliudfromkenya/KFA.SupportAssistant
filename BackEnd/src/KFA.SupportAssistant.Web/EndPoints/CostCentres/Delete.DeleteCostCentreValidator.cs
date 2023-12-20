namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteCostCentreValidator : Validator<DeleteCostCentreRequest>
{
  public DeleteCostCentreValidator()
  {
    //RuleFor(x => x.CostCentreCode)
    //  .NotEmpty();
  }
}
