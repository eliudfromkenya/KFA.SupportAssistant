using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

public record UpdateComputerAssetsWriteOffRequest
{
  public const string Route = "/computer_assets_write_offs/{writeOffID}";
  public string? AssetDetailID { get; set; }
  public string? Description { get; set; }
  public string? Narration { get; set; }
  public string? ReasonForWriteOff { get; set; }
  [Required]
  public string? WriteOffID { get; set; }
  public global::System.DateTime? WriteOffDate { get; set; }
}
