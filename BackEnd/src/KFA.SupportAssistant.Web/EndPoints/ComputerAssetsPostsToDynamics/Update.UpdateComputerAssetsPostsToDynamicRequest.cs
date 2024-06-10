using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

public record UpdateComputerAssetsPostsToDynamicRequest
{
  public const string Route = "/computer_assets_posts_to_dynamics/{postID}";
  public string? AssetDetailID { get; set; }
  public global::System.DateTime? DateGenerated { get; set; }
  public string? Description { get; set; }
  [Required]
  public string? PostID { get; set; }
  [Required]
  public bool? Posted { get; set; }
}
