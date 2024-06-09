using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

public class CreateComputerAssetsPostsToDynamicRequest
{
  public const string Route = "/computer_assets_posts_to_dynamics";
  public string? AssetDetailID { get; set; }
  public global::System.DateTime? DateGenerated { get; set; }
  public string? Description { get; set; }
  [Required]
  public string? PostID { get; set; }
  [Required]
  public bool? Posted { get; set; }
}
