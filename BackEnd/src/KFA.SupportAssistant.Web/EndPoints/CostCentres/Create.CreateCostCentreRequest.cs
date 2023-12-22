using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

public class CreateCostCentreRequest
{
  public const string Route = "/cost_centres";

  [Required]
  public string? CostCentreCode { get; set; }

  [Required]
  public string? Description { get; set; }

  public string? Narration { get; set; }
  public string? Region { get; set; }
  public string? SupplierCodePrefix { get; set; }
}
