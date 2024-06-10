using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

public class PatchComputerAssetsMaintainceReceivingRequest : JsonPatchDocument<ComputerAssetsMaintainceReceivingDTO>, IPlainTextRequest
{
  public const string Route = "/computer_assets_maintaince_receivings/{receiveID}";

  public static string BuildRoute(string receiveID) => Route.Replace("{receiveID}", receiveID);

  public string ReceiveID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ComputerAssetsMaintainceReceivingDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ComputerAssetsMaintainceReceivingDTO>>(Content)!;
}
