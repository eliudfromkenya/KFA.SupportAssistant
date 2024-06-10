using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

public class PatchComputerAssetGroupRequest : JsonPatchDocument<ComputerAssetGroupDTO>, IPlainTextRequest
{
  public const string Route = "/computer_asset_groups/{groupID}";

  public static string BuildRoute(string groupID) => Route.Replace("{groupID}", groupID);

  public string GroupID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerAssetGroupDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerAssetGroupDTO>>(Content)!;
}
