namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

public record DeleteCostCentreRequest
{
  public const string Route = "/cost_centres/{costCentreCode}";
  public static string BuildRoute(string? costCentreCode) => Route.Replace("{costCentreCode}", costCentreCode);
  //[QueryParam]
  public string? CostCentreCode { get; set; }
}
