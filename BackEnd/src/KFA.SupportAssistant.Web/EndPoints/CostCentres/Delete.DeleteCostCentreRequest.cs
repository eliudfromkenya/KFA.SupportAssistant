namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

public record DeleteCostCentreRequest
{
  public const string Route = "/cost_centres/{CostCentreCode}";
  public static string BuildRoute(string? costCentreCode) => Route.Replace("{CostCentreCode}", costCentreCode);
  //[QueryParam]
  public string? CostCentreCode { get; set; }
}
