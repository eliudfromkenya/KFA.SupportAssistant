using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetDetails;

public class PatchComputerAssetDetailRequest : JsonPatchDocument<ComputerAssetDetailDTO>, IPlainTextRequest
{
  public const string Route = "/computer_asset_details/{assetID}";

  public static string BuildRoute(string assetID) => Route.Replace("{assetID}", assetID);

  public string AssetID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerAssetDetailDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerAssetDetailDTO>>(Content)!;
}
