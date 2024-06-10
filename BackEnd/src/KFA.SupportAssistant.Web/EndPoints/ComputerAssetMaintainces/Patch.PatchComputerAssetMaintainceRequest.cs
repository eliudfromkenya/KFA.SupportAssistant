using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetMaintainces;

public class PatchComputerAssetMaintainceRequest : JsonPatchDocument<ComputerAssetMaintainceDTO>, IPlainTextRequest
{
  public const string Route = "/computer_asset_maintainces/{maintainceID}";

  public static string BuildRoute(string maintainceID) => Route.Replace("{maintainceID}", maintainceID);

  public string MaintainceID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerAssetMaintainceDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerAssetMaintainceDTO>>(Content)!;
}
