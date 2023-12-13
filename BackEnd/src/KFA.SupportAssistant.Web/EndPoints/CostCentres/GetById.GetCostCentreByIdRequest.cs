namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

public class GetCostCentreByIdRequest
{
  public const string Route = "/cost_centres/{costCentreCode}";

  public static string BuildRoute(string? costCentreCode) => Route.Replace("{costCentreCode}", costCentreCode);

  public string? CostCentreCode { get; set; }
}
