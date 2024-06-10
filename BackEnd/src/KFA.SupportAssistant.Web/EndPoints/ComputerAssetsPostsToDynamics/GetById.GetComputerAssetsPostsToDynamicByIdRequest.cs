namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

public class GetComputerAssetsPostsToDynamicByIdRequest
{
  public const string Route = "/computer_assets_posts_to_dynamics/{postID}";

  public static string BuildRoute(string? postID) => Route.Replace("{postID}", postID);

  public string? PostID { get; set; }
}
