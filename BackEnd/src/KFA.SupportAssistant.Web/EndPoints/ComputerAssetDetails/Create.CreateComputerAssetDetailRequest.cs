using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetDetails;

public class CreateComputerAssetDetailRequest
{
  public const string Route = "/computer_asset_details";

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
