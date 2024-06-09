using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetDetails;

public record UpdateComputerAssetDetailRequest
{
  public const string Route = "/computer_asset_details/{assetID}";
  [Required]
  public string? AssetID { get; set; }
  [Required]
  public string? AssetName { get; set; }
  public string? Description { get; set; }
  [Required]
  public string? GroupID { get; set; }
  public string? SerialNumber { get; set; }
  public string? State { get; set; }
}
