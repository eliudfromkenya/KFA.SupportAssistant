using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

public class ListCostCentreRequest
{
  public const string Route = "/cost_centres";

  [Required]
  public int? Take { get; set; }

  [Required]
  public int? Skip { get; set; }
}
