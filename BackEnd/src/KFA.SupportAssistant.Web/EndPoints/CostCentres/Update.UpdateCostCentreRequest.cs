using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

public record UpdateCostCentreRequest
{
  public const string Route = "/cost_centres/{costCentreCode}";

  public static string BuildRoute(string costCentreCode) => Route.Replace("{costCentreCode}", costCentreCode);

  //[Required]
  public string? CostCentreCode { get; set; }

  [Required]
  public string? Description { get; set; }

  public string? Narration { get; set; }

  [Required]
  public string? Region { get; set; }

  public string? SupplierCodePrefix { get; set; }
}
