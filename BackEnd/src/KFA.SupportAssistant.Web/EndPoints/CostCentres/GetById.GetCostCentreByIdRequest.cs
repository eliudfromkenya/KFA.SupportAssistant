namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

public class GetCostCentreByIdRequest
{
  public const string Route = "/cost_centres/{costCentreCode}";

  public static string BuildRoute(string? costCentreCode) => Route.Replace("{costCentreCode}", costCentreCode);

  public string? CostCentreCode { get; set; }
}
