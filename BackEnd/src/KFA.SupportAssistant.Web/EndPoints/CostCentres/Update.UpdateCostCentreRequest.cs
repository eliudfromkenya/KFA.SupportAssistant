using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

public record UpdateCostCentreRequest
{
  public const string Route = "/cost_centres/{id}";

  public static string BuildRoute(string id) => Route.Replace("{id}", id);

  [Required]
  public string? Id { get; set; }

  [Required]
  public string? Description { get; set; }

  public string? Narration { get; set; }

  [Required]
  public string? Region { get; set; }

  public string? SupplierCodePrefix { get; set; }


}
