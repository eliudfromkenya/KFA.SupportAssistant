using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

public class CostCentreListRequest
{
  public const string Route = "/cost_centres";

  [Required]
  [DefaultValue("Skip Value")]
  public int? Skip { get; set; }

  [Required]
  [DefaultValue("Count Value")]
  public int? Count { get; set; }

  [DefaultValue("Param Value")]
  public string? Param { get; set; }
}
