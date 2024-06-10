using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceAccessories;

public class PatchComputerAssetsMaintainceAccessoryRequest : JsonPatchDocument<ComputerAssetsMaintainceAccessoryDTO>, IPlainTextRequest
{
  public const string Route = "/computer_assets_maintaince_accessories/{accessoryID}";

  public static string BuildRoute(string accessoryID) => Route.Replace("{accessoryID}", accessoryID);

  public string AccessoryID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerAssetsMaintainceAccessoryDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerAssetsMaintainceAccessoryDTO>>(Content)!;
}
