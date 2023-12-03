namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

public record DeleteCostCentreRequest
{
  public const string Route = "/cost_centres/{costCentreCode}";
  public static string BuildRoute(string? costCentreId) => Route.Replace("{costCentreCode}", costCentreId);

  public string? CostCentreCode { get; set; }
}
