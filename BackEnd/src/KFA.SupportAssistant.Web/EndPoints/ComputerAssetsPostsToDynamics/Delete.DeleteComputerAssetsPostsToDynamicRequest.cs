namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

public record DeleteComputerAssetsPostsToDynamicRequest
{
  public const string Route = "/computer_assets_posts_to_dynamics/{postID}";
  public static string BuildRoute(string? postID) => Route.Replace("{postID}", postID);
  public string? PostID { get; set; }
}
