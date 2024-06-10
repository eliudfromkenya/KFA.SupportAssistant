using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

public class PatchComputerAssetTransferRequest : JsonPatchDocument<ComputerAssetTransferDTO>, IPlainTextRequest
{
  public const string Route = "/computer_asset_transfers/{transferID}";

  public static string BuildRoute(string transferID) => Route.Replace("{transferID}", transferID);

  public string TransferID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerAssetTransferDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerAssetTransferDTO>>(Content)!;
}
