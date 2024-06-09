using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsStatuses;

public class PatchComputerAssetsStatusRequest : JsonPatchDocument<ComputerAssetsStatusDTO>, IPlainTextRequest
{
  public const string Route = "/computer_assets_statuses/{assetsStatusID}";

  public static string BuildRoute(string assetsStatusID) => Route.Replace("{assetsStatusID}", assetsStatusID);

  public string AssetsStatusID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerAssetsStatusDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerAssetsStatusDTO>>(Content)!;
}
