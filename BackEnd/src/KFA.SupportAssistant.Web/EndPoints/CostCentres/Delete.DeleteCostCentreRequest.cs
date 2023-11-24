namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

public record DeleteCostCentreRequest
{
  public const string Route = "/cost_centres/{id}";
  public static string BuildRoute(string? costCentreId) => Route.Replace("{id}", costCentreId);

  public string? Id { get; set; }
}
