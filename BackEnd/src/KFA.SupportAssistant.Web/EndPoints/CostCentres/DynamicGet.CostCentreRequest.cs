using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

public class DynamicGetCostCentreRequest
{
  public const string Route = "/cost_centres/dynamically";

  public ListParam? ListParam { get; init; } = null;
}
