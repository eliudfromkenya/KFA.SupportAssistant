using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

public class PatchComputerAssetsPostsToDynamicRequest : JsonPatchDocument<ComputerAssetsPostsToDynamicDTO>, IPlainTextRequest
{
  public const string Route = "/computer_assets_posts_to_dynamics/{postID}";

  public static string BuildRoute(string postID) => Route.Replace("{postID}", postID);

  public string PostID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerAssetsPostsToDynamicDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerAssetsPostsToDynamicDTO>>(Content)!;
}
