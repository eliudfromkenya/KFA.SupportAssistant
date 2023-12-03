using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

public class CreateCostCentreRequest
{
  public const string Route = "/cost_centres";

  public string? CostCentreCode { get; set; }

  [Required]
  public string? Description { get; set; }

  public string? Narration { get; set; }

  [Required]
  public string? Region { get; set; }

  public string? SupplierCodePrefix { get; set; }
  public bool? IsActive { get; set; } = true;
}
