using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsStatuses;

public record UpdateComputerAssetsStatusRequest
{
  public const string Route = "/computer_assets_statuses/{assetsStatusID}";
  public string? AssetDetailID { get; set; }
  [Required]
  public string? AssetsStatusID { get; set; }
  public global::System.DateTime? Date { get; set; }
  public string? Description { get; set; }
  public string? DoneBy { get; set; }
  public string? Status { get; set; }
}
